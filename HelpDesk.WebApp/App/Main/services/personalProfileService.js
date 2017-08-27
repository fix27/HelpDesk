(function () {
    var app = angular.module('app');
    app.factory('personalProfileService', ['$http', 'localizedWebAPIService',
        function ($http, localizedWebAPIService) {

            var factory = {};

            
            var _get = function () {
                return $http.get(localizedWebAPIService.get("PersonalProfile/Get")).then(function (results) {
                    return results;
                });
            };

            var _getListPost = function (name) {
                return $http.get(localizedWebAPIService.get("PersonalProfile/GetListPost?name=" + name)).then(function (results) {
                    return results;
                });
            };

            var _getListOrganization = function (name) {
                return $http.get(localizedWebAPIService.get("PersonalProfile/GetListOrganization?name=" + name)).then(function (results) {
                    return results;
                });
            };

                       
            var _save = function (personalProfile) {
                return $http.post(localizedWebAPIService.get("PersonalProfile/Save"), JSON.stringify(personalProfile)).then(function (results) {
                    return results;
                });
            };

           
            factory.save        = _save;
            factory.get         = _get;
            factory.getListPost = _getListPost;
            factory.getListOrganization     = _getListOrganization;
            
            return factory;

        }
    ]);
})();