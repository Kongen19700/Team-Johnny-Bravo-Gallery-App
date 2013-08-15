/// <reference path="class.js" />
/// <reference path="html-escape.js" />
/// <reference path="httpRequester.js" />
/// <reference path="service-manager.js" />
/// <reference path="jquery-2.0.3.js" />

var gameRefreshInterval;

var SiteController = Class.create({
    init: function (rootUrl, containerSelector) {
        this.rootUrl = rootUrl;
        this.root = $(containerSelector);
        this.service = services.getMainService(rootUrl);
    },

    render: function () {
        var self = this;

        this.attachEventHandlers();

        if (this.service.isUserLoggedIn()) {
            $('body').prepend('<div id="logged-user">'
                   + '<span id="user-nickname">' + this.service.getNickname() + '</span>' +
                   '<button id="logout" class="btn btn-info">Logout</button></div>');

            PUBNUB.subscribe({
                channel: "testuser_notifications",
                callback: function (message) {
                    document.getElementById('messagesArea').innerHTML += message + '\n';
                },
                error: function (data) { console.log(data) }
            });

            this.service.gallery.getAllGalleries(
                function (data) {
                    self.renderGalleries(data)
                },
                function (data) {
                    self.errorHandler(data);
                }
            );
        }
        else {
            this.renderLogInForm();
        }
    },

    renderLogInForm: function () {
        this.root.html(
            '<div id="login-form-holder">' +
                '<form class="navbar-form pull-left">' +
                    '<input type="text" id="tb-login-username" placeholder="username"/>' +
                    '<button id="btn-login" type="submit" class="btn btn-info">Login</button>' +
                '</form>' +
            '</div>');
    },

    renderGalleries: function (receivedData) {

        var galleriesHtml = '<div id="galleries">';

        for (var i = 0; i < receivedData.length; i++) {
            var currentGallery = receivedData[i];

            galleriesHtml +=
                '<h2 class="user-gallery" data-gallery-id="' + currentGallery.AlbumId + '">' + currentGallery.Title + '</h2>';
        }

        galleriesHtml += '</div>';

        this.root.html(galleriesHtml);
    },

    renderAlbum: function (receivedData) {

        var albumHtml =
        '<div id="gallery">' +
             '<div id="create-upload">' +
                '<div id="upload">' +
                    '<form method="post" style="width:45%"' +
                    ' action="http://team-johnny-bravo-gallery-app.apphb.com/images/PostImage" id="upload-form"> ' +
                    //' action="http://localhost:51015/images/PostImage" id="upload-form"> ' +
                    '<div class="demo-section">                                                              ' +
                        '<input name="title" id="title" type="text" placeholder="Image title" />' +
                        '<input name="file" id="filesForm" type="file"/>                                        ' +
                        '<input name="albumId" id="albumId" value="' + receivedData.AlbumId + '" type="hidden" />' +
                        '<input type="submit" value="Submit" id="submit-btn" class="k-button btn btn-info" />' +
                    '</div>                                                                                  ' +
                    '</form>                                                                                 ' +
                    '<script>                                                                                ' +
                    '$(document).ready(function () {                                                         ' +
                    '$("#filesForm").kendoUpload();                                                          ' +
                    '});                                                                                     ' +
                    '</script>                                                                               ' +
               '</div>' + // upload 
                '<div id="create-album">                                                               ' +
                '    <input type="text" id="tb-new-album-title" placeholder="album name"/> ' +
                '    <button id="btn-create-album" class="btn btn-info">Add Album</button>      ' +
                '</div>' + //create 
            '</div>' + //create upload 
            '<div id="album-content" data-current-album-id="' + receivedData.AlbumId + '">';


        albumHtml += '<h2>' + receivedData.Title.htmlEscape() + '</h2>';

        var i = 0;

        if (receivedData.SubAlbums && receivedData.SubAlbums.length > 0) {
            debugger;
            for (i = 0; i < receivedData.SubAlbums.length; i++) {
                var currentAlbum = receivedData.SubAlbums[i];
                albumHtml +=
                '<div class="sub-album" data-album-id="' + currentAlbum.AlbumId + '">' +
                '<h3>' + currentAlbum.Title.htmlEscape() + '</h3>' +
                '</div>';
            }
        }

        if (receivedData.Images && receivedData.Images.length) {
            for (i = 0; i < receivedData.Images.length; i++) {
                var currentImage = receivedData.Images[i];
                albumHtml +=
                '<div class="sub-image" data-image-id="' + currentImage.ImageId + '">' +
                //'<h4 class="image-title">'+ currentImage.Title + '</h4>' +
                '<img src="' + currentImage.Url + '" alt="image" class="thumb" />' +
                '</div>';
            }
        }

        albumHtml += '</div>'; //album-content
        albumHtml += '</div>'; //gallery

        this.root.html(albumHtml);
    },

    renderImage: function (receivedData) {
        var imageContainerHtml = '<div id="image-container">' +
            //'<h3>'+ receivedData.Title + '</h3>'+
            '<img src="' + receivedData.Url + '" alt="image" data-image-id="' +
                receivedData.ImageId + '" class="large"/>' +
        '<div id="image-comments">'
        if (receivedData.Comments && receivedData.Comments.length > 0) {
            for (var i = 0; i < receivedData.Comments.length; i++) {
                var currentComment = receivedData.Comments[i];
                imageContainerHtml += '<div class="comment" >' +
                    '<span class="comment-text">' + currentComment.Text + '</span>' +
                    '<span class="comment-author"> by ' + currentComment.Username + '</span></div>';
            }
        }
        imageContainerHtml += '</div>';

        imageContainerHtml +=
            '<div id="new-comment-form">' +
                '<textarea id="comment-textarea">Enter your comment here...</textarea>' +
                '<button id="btn-post-new-comment" class="btn btn-info">Post comment</button>' +
            '</div>';

        imageContainerHtml += '</div>';

        this.root.html(imageContainerHtml);
    },


    //Events

    attachEventHandlers: function () {
        var self = this;

        this.root.on('click', '#btn-login', function () {
            var username = $('#tb-login-username', self.root).val();

            self.service.user.login(username, function (receivedData) {

                $('body').prepend('<div id="logged-user">'
                    + '<span id="user-nickname">' + receivedData.Username.htmlEscape() + '</span>' +
                    '<button id="logout" class="btn btn-info">Logout</button></div>');
                self.service.gallery.getAllGalleries(
                    function (data) {
                        self.renderGalleries(data)
                    },
                    function (data) {
                        self.errorHandler(data);
                    }
                );

            }, function (receivedData) {
                self.errorHandler(receivedData);
            });

            return false;
        });

        this.root.on('click', '.user-gallery', function () {
            var target = $(this);
            var galleryId = target.attr("data-gallery-id");

            self.service.album.getAlbum(galleryId, function (receivedData) {
                self.renderAlbum(receivedData);
            }, function (receivedData) {
                self.errorHandler(receivedData);
            });

            return false;
        })

        this.root.on('click', '.sub-album', function () {
            var target = $(this);
            var albumId = target.attr('data-album-id');

            self.service.album.getAlbum(albumId, function (receivedData) {
                debugger;
                self.renderAlbum(receivedData);
            }, function (receivedData) {
                self.errorHandler(receivedData);
            });

            return false;
        })

        this.root.on('click', '.sub-image', function () {
            debugger;
            var target = $(this);
            var imageId = target.attr('data-image-id');

            self.service.image.getImage(imageId, function (receivedData) {
                self.renderImage(receivedData);
            }, function (receivedData) {
                self.errorHandler(receivedData);
            });

            return false;
        })

        this.root.on('click', '#btn-post-new-comment', function () {
            debugger;
            var commentText = $('#comment-textarea').val();
            var userId = self.service.getUserid();
            var imageId = $('#image-container > img.large').attr('data-image-id');

            self.service.comment.postComment(commentText, imageId, userId, function (receivedData) {
                self.service.image.getImage(imageId, function (receivedData) {
                    self.renderImage(receivedData);
                }, function (receivedData) {
                    self.errorHandler(receivedData);
                });
            }, function (receivedData) {
                self.errorHandler(receivedData);
            });

            return false;
        })

        this.root.on('click', '#btn-create-album', function () {
            var title = $('#tb-new-album-title').val();
            var parentAlbumId = $('#album-content').attr('data-current-album-id');
            var userId = self.service.getUserid();

            self.service.album.postAlbum(userId, title, parentAlbumId,
            function (receivedData) {
                self.service.album.getAlbum(parentAlbumId, function (receivedData) {
                    self.renderAlbum(receivedData);
                }, function (receivedData) {
                    self.errorHandler(receivedData);
                });
            }, function (receivedData) {
                self.errorHandler(receivedData);
            });
        })

        this.root.on('click', '#btn-close-error-message', function () {
            $('.error-message').fadeOut(1000, function () { $('.error-message').remove() });
        });

        $('body').on('click', '#logout', function () {
            self.service.clearUserData();
            self.root.html("");
            $('#logged-user').remove();
            self.renderLogInForm();

            return false;
        })
    },

    errorHandler: function (receivedData) {
        $('.error-message').remove();
        if (receivedData.responseJSON) {
            this.root.prepend('<div class="error-message">' + '<span id="btn-close-error-message">X</span>'
                + receivedData.responseJSON.Message + '</div>');
        }
    },

});