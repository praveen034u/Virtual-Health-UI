window.showAuth0Login = () => {
    const container = document.getElementById("auth0-login-container");

    if (container && !container.hasChildNodes()) {
        var lock = new Auth0Lock('V9Una2guriD6USXmVHCn4LVRZX9Ao479', 'dev-jbrriuc5vyjmiwtx.us.auth0.com', {
            auth: {
                redirect: false,
                responseType: 'token id_token',
                audience: 'https://dev-jbrriuc5vyjmiwtx.us.auth0.com/userinfo',
                scope: 'openid profile email'
            }
        });

        lock.on("authenticated", function (authResult) {
            lock.getUserInfo(authResult.accessToken, function (error, profile) {
                if (error) {
                    console.log("Error fetching profile:", error);
                    return;
                }

                sessionStorage.setItem("access_token", authResult.accessToken);
                sessionStorage.setItem("id_token", authResult.idToken);
                sessionStorage.setItem("user_profile", JSON.stringify(profile));

                console.log("Auth success, invoking Blazor method...");

                DotNet.invokeMethodAsync("VirtualHealth.UI", "OnAuthSuccess")
                    .catch(err => console.error("Blazor callback failed", err));
            });
        });

        lock.show();
    }
};

window.logoutAuth0 = (logoutUrl) => {
    sessionStorage.removeItem("access_token");
    sessionStorage.removeItem("id_token");
    sessionStorage.removeItem("user_profile");
    sessionStorage.clear();

    // Redirect to Auth0 logout page
    window.location.href = logoutUrl;
};

window.secureStorage = {
    setItem: function (key, value) {
        sessionStorage.setItem(key, value);
    },
    getItem: function (key) {
        return sessionStorage.getItem(key);
    },
    removeItem: function (key) {
        sessionStorage.removeItem(key);
    }
};
