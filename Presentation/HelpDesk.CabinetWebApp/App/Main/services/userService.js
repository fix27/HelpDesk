(function () {
    var app = angular.module('app');
    app.factory('userService', ['$http', 'localizedWebAPIService',
        function ($http, localizedWebAPIService) {

            var factory = {};

            var _get = function () {
                return $http.get(localizedWebAPIService.get("User/Get")).then(function (results) {
                    return results;
                });
            };

            var _getListSubscribeStatus = function () {
                return $http.get(localizedWebAPIService.get("User/GetListSubscribeStatus")).then(function (results) {
                    return results;
                });
            };

            var _changeSubscribeRequestState = function (requestState) {
                return $http.post(localizedWebAPIService.get("User/ChangeSubscribeRequestState"), JSON.stringify({ requestState: requestState })).then(function (results) {
                    return results;
                });
            };

            var _changeSubscribe = function () {
                return $http.post(localizedWebAPIService.get("User/ChangeSubscribe")).then(function (results) {
                    return results;
                });
            };

            factory.get = _get;
            factory.getListSubscribeStatus = _getListSubscribeStatus;
            factory.changeSubscribeRequestState = _changeSubscribeRequestState;
            factory.changeSubscribe = _changeSubscribe;

            return factory;

        }
    ]);
})();