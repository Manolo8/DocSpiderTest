function toggleMenu(open = null) {
    const self = $('.toggle_menu .icon');
    const menu = $('.c_menu')

    const shouldOpen = open === null ? !menu.hasClass('b_open') : open;

    if (open === null)
        setLocalStorage('menu_open', shouldOpen);

    if (shouldOpen) {
        menu.addClass('b_open');
        menu.removeClass('b_close');
        self.html('menu_open');
    } else {
        menu.removeClass('b_open');
        menu.addClass('b_close');
        self.html('menu');
    }
}

function getLocalStorage(key, def) {
    const str = localStorage.getItem(key);

    if (!str)
        return def;

    return JSON.parse(str);
}

function setLocalStorage(key, value) {
    const json = JSON.stringify(value);

    localStorage.setItem(key, json);
}

function isViewPortLessThan(width) {
    return window.innerWidth < width;
}

$(document).ready(() => {
    const def = getLocalStorage('menu_open', false);
    toggleMenu(isViewPortLessThan(768) ? false : def);
});