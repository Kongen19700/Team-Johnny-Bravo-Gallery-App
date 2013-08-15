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

        //this.attachEventHandlers();

        if (this.service.isUserLoggedIn()) {
            this.renderGameUI();
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

    renderGalleries: function(receivedData){
        alert('hi');
    },

    //renderGameUI: function () {
    //    var self = this;

    //    if (gameRefreshInterval) {
    //        clearInterval(gameRefreshInterval);
    //    }
    //    gameRefreshInterval = setInterval(function () {
    //        self.renderActiveGames();
    //        self.renderOpenGames();
    //        self.renderScoreboard();
    //    }, 10000);

    //    this.root.html(
    //        '<form id="logout-form">' +
    //            '<span id="nickname-display">' + this.service.getNickname() + '</span>' +
    //            '<button id="btn-logout">Logout</button>' +
    //        '</form>' +
    //        '<div id="create-new-game"></div>' +
    //        '<div id="active-games"></div>' +
    //        '<div id="open-games"></div>' +
    //        '<div id="game-field" style="display:none"></div>' +
    //        '<div id="scoreboard"></div>'
    //    );
    //    this.renderCreateNewGame();
    //    this.renderActiveGames();
    //    this.renderOpenGames();
    //    this.renderGameField();
    //    this.renderScoreboard();
    //},

    //renderActiveGames: function () {
    //    var self = this;
    //    var activeGamesContainer = $('#active-games');

    //    this.service.game.active(function (receivedData) {
    //        activeGamesContainer.html('<h2>Active Games:<h2>');
    //        var list = '<ul>';
    //        for (var i = 0; i < receivedData.length; i++) {

    //            var startButtonDisplay = "";
    //            if (self.service.getNickname() !== receivedData[i].creator ||
    //                receivedData[i].status !== "full") {
    //                startButtonDisplay = "none";
    //            }
    //            var showFieldButtonDisplay = "";
    //            if (receivedData[i].status !== "in-progress") {
    //                showFieldButtonDisplay = "none";
    //            }

    //            list +=
    //                '<li>' +
    //                    '<a href="#" data-active-game-id="' + receivedData[i].id + '">' + receivedData[i].title.htmlEscape() + '</a>' +
    //                    '<span class="game-creator"> by ' + receivedData[i].creator + '</span>' +
    //                    '<span class="game-status"> ' + receivedData[i].status + '</span>' +
    //                    '<button id="start-full-game" data-active-game-id="' + receivedData[i].id +
    //                    '" style="display:' + startButtonDisplay + '">Start</button>' +
    //                    '<button id="show-game-field" data-active-game-id="' + receivedData[i].id +
    //                    '" style="display:' + showFieldButtonDisplay + '">Show Field</button>' +
    //            '</li>';
    //        }
    //        list += '</ul>';
    //        activeGamesContainer.append(list);
    //    }, function (receivedData) {
    //        self.errorHandler(receivedData);
    //    });
    //},

    //renderOpenGames: function () {
    //    var self = this;
    //    var openGamesContainer = $('#open-games');

    //    this.service.game.open(function (receivedData) {
    //        openGamesContainer.html('<h2>Open Games:</h2>');
    //        var list = '<ul>';
    //        for (var i = 0; i < receivedData.length; i++) {
    //            list +=
    //                '<li>' +
    //                    '<a href="#" data-open-game-id="' + receivedData[i].id + '">' + receivedData[i].title.htmlEscape() + '</a>' +
    //                    '<span class="game-creator"> by ' + receivedData[i].creator + '</span>' +
    //                '</li>';
    //        }

    //        list += '</ul>';
    //        openGamesContainer.append(list);
    //    }, function (receivedData) {
    //        self.errorHandler(receivedData);
    //    });
    //},

    //renderCreateNewGame: function () {
    //    var newGameContainer = $('#create-new-game');
    //    newGameContainer.html('<h2>New Game:</h2>');
    //    newGameContainer.append(
    //        '<form id="create-new-game-form">' +
    //            '<label for="tb-new-game-title">Title:</label>' +
    //            '<input type="text" id="tb-new-game-title" />' +
    //            '<label for="tb-new-game-password">Password:</label>' +
    //            '<input type="password" id="tb-new-game-password" />' +
    //            '<button id="btn-create-new-game">Create</button>' +
    //        '</form>'
    //    );
    //},

    //renderScoreboard: function () {
    //    var self = this;
    //    var scoreboardContainer = $('#scoreboard');
    //    scoreboardContainer.html('<h2>Scoreboard:</h2>')

    //    self.service.user.scores(function (receivedData) {
    //        receivedData.sort(function (a, b) {
    //            return (b.score - a.score);
    //        });

    //        var list = '<ol>';
    //        for (var i = 0; i < receivedData.length && i < 5; i++) {
    //            list +=
    //                '<li>' +
    //                    '<span class="nickname">' + receivedData[i].nickname + '</span> - ' +
    //                    '<span class="score"> ' + receivedData[i].score + '</span>' +
    //                '</li>';
    //        }

    //        scoreboardContainer.append(list);
    //    }, function (receivedData) {
    //        self.errorHandler(receivedData);
    //    });
    //},

    //renderGameField: function (receivedData) {
    //    var self = this;
    //    if (!receivedData) {
    //        return;
    //    }
    //    //if (!receivedData) {
    //    //    if ($('#game-field').length !== 0) {
    //    //        if ($('#game-field').data('game-id')) {
    //    //            var gameData = {
    //    //                gameId: $('#game-field').data('game-id')
    //    //            }
    //    //            self.service.game.field(gameData, function (response) {
    //    //                receivedData = response;
    //    //            }, function (response) {
    //    //                self.errorHandler(response);
    //    //                return;
    //    //            })
    //    //        }
    //    //        else {
    //    //            return false;
    //    //        }
    //    //    }
    //    //    else {
    //    //        return false;
    //    //    }
    //    //};

    //    var gameField = $('#game-field');
    //    gameField.attr('data-game-id', receivedData.gameId);
    //    gameField.fadeIn(1500);
    //    gameField.html(
    //        '<h2>Game Field:</h2>' +
    //        '<h3>' + receivedData.title + '</h3>' +
    //        '<h4><span class="red-nickname">' + receivedData.red.nickname + '</span> vs <span class="blue-nickname">' + receivedData.blue.nickname + '</span></h4>' +
    //        '<h5 class="in-turn-' + receivedData.inTurn + '">In turn: ' + receivedData.inTurn + '</h5>' +
    //        '<span id="btn-close-game-field">X</span>'
    //        );

    //    if (receivedData.red.nickname === this.service.getNickname()) {
    //        this.colorForCurrentGame = "red";
    //    }
    //    else if (receivedData.blue.nickname === this.service.getNickname()) {
    //        this.colorForCurrentGame = "blue";
    //    }

    //    this.colorInTurn = receivedData.inTurn;

    //    //building field
    //    var i = 0;
    //    var j = 0;

    //    var fieldRowsAndCols = $('<div id="field"></div>');
    //    for (i = 0; i < 9; i++) {
    //        var currentRow = $('<ul class="game-field-row"></ul>')
    //        for (j = 0; j < 9; j++) {
    //            currentRow.append('<li class="cell" data-x-coord="' + j + '" data-y-coord="' + i + '"></li>');
    //        }
    //        fieldRowsAndCols.append(currentRow);
    //    }
    //    gameField.append(fieldRowsAndCols);

    //    var redUnits = receivedData.red.units;
    //    this.placeUnits(redUnits, 'red-unit');
    //    var blueUnits = receivedData.blue.units;
    //    this.placeUnits(blueUnits, 'blue-unit');
    //},

    //placeUnits: function (units, unitClass) {
    //    for (i = 0; i < units.length; i++) {
    //        var x = units[i].position.x;
    //        var y = units[i].position.y;
    //        var cellToPlace = $('li.cell[data-x-coord=' + x + '][data-y-coord=' + y + ']');
    //        cellToPlace.addClass('unit');
    //        cellToPlace.addClass(unitClass);
    //        cellToPlace.addClass(units[i].type + "-unit");
    //        cellToPlace.attr('data-unit-id', units[i].id);
    //    }
    //},

    attachEventHandlers: function () {
        var self = this;

        this.root.on('click', '#btn-login', function () {
            var username = $('#tb-login-username', self.root).val();

            self.service.user.login(username, function (receivedData) {
                self.renderGalleries(receivedData)
            }, function (receivedData) {
                self.errorHandler(receivedData);
            });
        });

        //this.root.on('click', '#btn-register', function () {

        //    var username = $('#tb-register-user', self.root).val();
        //    var password = $('#tb-register-pass', self.root).val();
        //    var nickname = $('#tb-register-nick', self.root).val();

        //    var userData = { username: username, password: password, nickname: nickname };
        //    self.service.user.register(userData, function () {
        //        self.renderGameUI();
        //    }, function (receivedData) {
        //        self.errorHandler(receivedData);
        //    });

        //    return false;
        //});

        //this.root.on('click', '#btn-login', function () {
        //    var username = $('#tb-login-user', self.root).val();
        //    var password = $('#tb-login-pass', self.root).val();

        //    var userData = { username: username, password: password, };

        //    self.service.user.login(userData, function () {
        //        self.renderGameUI();
        //    }, function (receivedData) {
        //        self.errorHandler(receivedData);
        //    });

        //    return false;
        //});

        //this.root.on('click', '#btn-logout', function () {
        //    self.service.user.logout(function () {
        //        self.renderLogInForm();
        //    }, function (receivedData) {
        //        self.errorHandler(receivedData);
        //    })

        //    return false;
        //})

        //this.root.on('click', '#btn-close-error-message', function () {
        //    $('.error-message').fadeOut(1000, function () { $('.error-message').remove() });
        //});

        //this.root.on('click', '#btn-switch-to-login-form', function () {
        //    $('fieldset#register-form').fadeOut(200, function () {
        //        $('fieldset#login-form').fadeIn(500);
        //    });
        //    $('#btn-switch-to-register-form').removeClass('selected');
        //    $('#btn-switch-to-login-form').addClass('selected');

        //    return false;
        //})

        //this.root.on('click', '#btn-switch-to-register-form', function () {
        //    $('fieldset#login-form').fadeOut(200, function () {
        //        $('fieldset#register-form').fadeIn(500);
        //    });
        //    $('#btn-switch-to-register-form').addClass('selected');
        //    $('#btn-switch-to-login-form').removeClass('selected');

        //    return false;
        //})

        //this.root.on('click', 'div#open-games a', function () {
        //    $('form#join-game-form').remove();
        //    var gameId = $(this).data('open-game-id');
        //    var joinGameForm = $('<form id="join-game-form"></form>');
        //    joinGameForm.html(
        //            '<label for="tb-join-game-password">Password:</label>' +
        //            '<input type="password" id="tb-join-game-password"/>' +
        //            '<button id="btn-join-game" data-game-id="' + gameId + '">Join</button>'
        //    );

        //    $(this).parent().append(joinGameForm);

        //    return false;
        //})

        //this.root.on('click', '#open-games #btn-join-game', function () {
        //    var gameId = $(this).data('game-id');
        //    var gameData = {
        //        id: gameId,
        //    }

        //    if ($('#tb-join-game-password').val()) {
        //        gameData.password = $('#tb-join-game-password').val();
        //    }

        //    self.service.game.join(gameData, function () {
        //        self.renderOpenGames();
        //        self.renderActiveGames();
        //    }, function (receivedData) {
        //        self.errorHandler(receivedData);
        //    })

        //    return false;
        //});

        //this.root.on('click', '#active-games #start-full-game', function () {
        //    var id = $(this).data('active-game-id');
        //    var gameData = {
        //        id: id,
        //    }

        //    self.service.game.start(gameData, function () {
        //        self.renderGameUI();
        //    }, function (receivedData) {
        //        self.errorHandler(receivedData);
        //    })
        //})

        //this.root.on('click', '#btn-create-new-game', function () {
        //    var title = $('#tb-new-game-title').val();
        //    var password = $('#tb-new-game-password').val();

        //    var gameData = {
        //        title: title,
        //    }
        //    if (password) {
        //        gameData.password = password;
        //    }

        //    self.service.game.create(gameData, function () {
        //        self.renderActiveGames();
        //    }, function (receivedData) {
        //        self.errorHandler(receivedData);
        //    });
        //});

        //this.root.on('click', '#active-games #show-game-field', function () {
        //    var target = $(this);
        //    var gameId = target.data('active-game-id');
        //    var gameData = {
        //        gameId: gameId
        //    }

        //    self.service.game.field(gameData, function (receivedData) {
        //        self.renderGameField(receivedData);
        //    }, function (receivedData) {
        //        self.errorHandler(receivedData);
        //    });

        //    if (gameRefreshInterval) {
        //        clearInterval(gameRefreshInterval);
        //    }
        //    gameRefreshInterval = setInterval(function () {
        //        self.renderActiveGames();
        //        self.renderOpenGames();
        //        self.renderScoreboard();

        //        self.service.game.field(gameData, function (receivedData) {
        //            self.renderGameField(receivedData);
        //        }, function (receivedData) {
        //            self.errorHandler(receivedData);
        //        });

        //    }, 10000);
        //})

        //this.root.on('click', '#btn-close-game-field', function () {
        //    $('#game-field').fadeOut(1500);
        //})

        //this.root.on('click', '.cell', function () {
        //    if (self.colorInTurn !== self.colorForCurrentGame) {
        //        return;
        //    }

        //    var target = $(this);

        //    var gameData;
        //    var unitId = $('.selected-cell').data('unit-id');
        //    var gameId = $('#game-field').data('game-id');
        //    var x = target.data('x-coord');
        //    var y = target.data('y-coord');

        //    if ($('.selected-cell').length == 0 && target.hasClass(self.colorForCurrentGame + '-unit')) {
        //        //select unit

        //        target.addClass('selected-cell');
        //    }
        //    else if (target.data('unit-id') === $('.selected-cell').first().data('unit-id')) {
        //        //click on same unit -> defend

        //        gameData = {
        //            gameId: gameId,
        //            unitId: target.data('unit-id'),
        //        }
        //        self.service.battle.defend(gameData, function () {
        //            alert('for debug purpose-> defended');
        //            self.renderGameField();
        //        }, function (receivedData) {
        //            self.errorHandler(receivedData);
        //        });
        //        $('.selected-cell').removeClass('.selectedCell');
        //    }
        //    else if (target.hasClass('unit') && !(target.hasClass(self.colorForCurrentGame + '-unit'))) {
        //        //click on enemy unit -> attack

        //        gameData = {
        //            gameId: gameId,
        //            unitId: unitId,
        //            positionX: x,
        //            positionY: y,
        //        }

        //        self.service.battle.attack(gameData, function () {
        //            alert('for debug purpose -> atacked');
        //            self.renderGameField();
        //        }, function (receivedData) {
        //            self.errorHandler(receivedData);
        //        });
        //        $('.selected-cell').removeClass('.selectedCell');
        //    }
        //    else if ($('.selected-cell').length !== 0
        //        && target.hasClass('cell') &&
        //        !(target.hasClass('red-unit'))
        //        && !(target.hasClass('blue-unit'))) {
        //        // empty cell -> move

        //        gameData = {
        //            gameId: gameId,
        //            unitId: unitId,
        //            positionX: x,
        //            positionY: y,
        //        };

        //        self.service.battle.move(gameData, function () {
        //            alert('for debug purpose -> moved');
        //            self.renderGameField();
        //        }, function (receivedData) {
        //            self.errorHandler(receivedData);
        //        })

        //        $('.selected-cell').removeClass('.selectedCell');
        //    }
        //})
    },

    errorHandler: function (receivedData) {
        $('.error-message').remove();
        if (receivedData.responseJSON) {
            this.root.prepend('<div class="error-message">' + '<span id="btn-close-error-message">X</span>'
                + receivedData.responseJSON.Message + '</div>');
        }
    },

});