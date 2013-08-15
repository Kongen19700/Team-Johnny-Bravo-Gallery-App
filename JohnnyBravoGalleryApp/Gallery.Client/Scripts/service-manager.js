/// <reference path="jquery-2.0.3.js" />
/// <reference path="class.js" />
/// <reference path="httpRequester.js" />

var services = (function () {

    var nickname = localStorage.getItem("gallery-app-nickname");
    var userId = localStorage.getItem("gallery-app-userId");

    function setUserData(nicknameParam, userIdparam) {
        nickname = nicknameParam;
        userId = userIdparam;
        localStorage.setItem("gallery-app-nickname", nickname);
        localStorage.setItem("gallery-app-userId", userId);
    }

    function clearUserData() {
        nickname = "";
        userId = "";
        localStorage.removeItem("gallery-app-nickname");
        localStorage.removeItem("gallery-app-userId");
    }

    var MainService = Class.create({
        init: function (url) {
            //http://team-johnny-bravo-gallery-app.apphb.com/api/
            this.rootUrl = url;
            this.user = new UserService(url);
            this.gallery = new GalleryService(url);
            this.album = new AlbumService(url);
            this.image = new ImageService(url);
            this.comment = new CommentService(url);
        },
        isUserLoggedIn: function () {
            if (nickname && userId) {
                return true;
            }
            else {
                return false;
            }
        },
        getNickname: function () {
            return nickna;
        },
        getUserid: function () {
            return userId;
        },
        clearUserData: function () {
            clearUserData();
        },
    })

    var UserService = Class.create({
        init: function (url) {
            //http://team-johnny-bravo-gallery-app.apphb.com/api/users/
            this.rootUrl = url + "users/";
        },
        login: function (nickname, successHandler, errorHandler) {
            var user = {
                username: nickname,
            };
            var url = this.rootUrl;

            httpRequester.postJSON(url, user, function (receivedData) {
                alert('success');
                setUserData(receivedData.nickname, receivedData.userId);
                successHandler(receivedData);
            }, function (receivedData) {
                alert('error');
                var a = receivedData;
                errorHandler(receivedData);
            });
        },
        logout: function (nickname) {
            clearUserData();
        }
    })

    var GalleryService = Class.create({
        init: function (url) {
            //http://team-johnny-bravo-gallery-app.apphb.com/api/galleries/
            this.rootUrl = url + "galleries/";
        },
        getUserGallery: function (userId, successHandler, errorHandler) {
            var url = this.rootUrl + userId + "/";
            httpRequester.getJSON(url, successHandler, errorHandler);
        },
    })

    var AlbumService = Class.create({
        init: function (url) {
            //http://team-johnny-bravo-gallery-app.apphb.com/api/albums/
            this.rootUrl = url + "album/";
        },
        getAlbum: function (id) {
            var url = this.rootUrl + id + "/";
            httpRequester.getJSON(url, successHandler, errorHandler);
        },
        postAlbum: function (userId, title, parentAlbumId) {
            var image = {
                userId: userId | 0,
                title: title,
                parentAlbumId: parentAlbumId | 0,
            };
            httpRequester.postJSON(url, image, successHandler, errorHandler);
        }
    })


    var ImageService = Class.create({
        init: function (url) {
            this.rootUrl = url + "images/"
        },
        getImage: function (id) {
            var url = this.rootUrl + id + "/";
            httpRequester.getJSON(url, succesHandler, errorHandler);
        },
        postImage: function () {

        },
    });

    var CommentService = Class.create({
        init: function (url) {
            this.rootUrl = url + "comments/"
        },
        postComment: function (text, imageId, userId) {
            var comment = {
                text: text,
                imageId: imageId | 0,
                userId: userId | 0,
            };

            httpRequester.postJSON(url, commen, succesHandler, errorHandler);
        }
    });

    return {
        getMainService: function (url) {
            return new MainService(url);
        }
    }
})();