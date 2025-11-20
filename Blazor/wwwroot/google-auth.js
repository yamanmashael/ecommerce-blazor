window.handleCredentialResponse = function (response) {
    const credential = response.credential;
    DotNet.invokeMethodAsync('Blazor', 'OnGoogleLogin', credential);
};

// wwwroot/js/localStorageInterop.js
window.localStorageInterop = {
    getItem: function (key) {
        return localStorage.getItem(key);
    },
    setItem: function (key, value) {
        localStorage.setItem(key, value);
    },
    removeItem: function (key) {
        localStorage.removeItem(key);
    }
};

