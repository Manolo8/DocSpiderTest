using System;
using Core.Application.Archives.Dtos.Patches;
using Core.Application.Auth.Impl;
using Core.Application.Auth.Services;
using Core.Application.Documents.Dtos.Filters;
using Core.Application.Documents.Dtos.Patches;
using Core.Application.Documents.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;
using Shared.Security;

namespace DocSpiderTest.Controllers {
    [Authorize]
    public class DocumentController : Controller {
        private readonly UserAuth          _user;
        private readonly DocumentService   _service;
        private readonly PermissionService _permission;

        public DocumentController(DocumentService   service,
                                  PermissionService permission,
                                  CurrentAuth       auth) {
            _service    = service;
            _permission = permission;
            _user       = auth.TryGet<UserAuth>();
        }

        public IActionResult Index([FromQuery] DocumentFilterDto filter) {
            filter.UserId = _user.Id;

            filter.SortColumn = "Title";
            filter.SortAsc    = true;

            var result = _service.SearchToViewList(filter);
            var tuple  = (filter, result);

            return View(tuple);
        }

        [HttpGet("Document/{id}")]
        public IActionResult Info(long id) {
            if (!_permission.IsOwnerOfDocument(_user.Id, id))
                return RedirectToAction("Index");

            var result = _service.FindToViewInfo(id);

            return View(result);
        }

        [HttpGet("Document/Editor/{id?}")]
        public IActionResult Editor([FromRoute] long? id) {
            if (id.HasValue && !_permission.IsOwnerOfDocument(_user.Id, (long) id))
                return RedirectToAction("Index");

            var model = id.HasValue ? _service.Model((long) id) : new DocumentDto {
                Archive = new ArchiveDto()
            };

            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost]
        public IActionResult Editor([FromForm] DocumentDto dto) {
            if (dto.Id != 0 && !_permission.IsOwnerOfDocument(_user.Id, dto.Id))
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
                return View(dto);

            dto.UserId = _user.Id;

            try {
                var result = dto.Id == 0 ?
                                 _service.Create(dto) :
                                 _service.Update(dto);

                return RedirectToAction("Editor", new {id = result.Id});
            }
            catch (ValidationFieldException x) {
                ModelState.AddModelError(x.Field, x.Error);
                return View(dto);
            }
        }

        [HttpGet("Document/Delete/{id}")]
        public IActionResult Delete(long id) {
            if (_permission.IsOwnerOfDocument(_user.Id, id))
                _service.Delete(id);

            return Ok();
        }
    }
}