(function () {
    var app = angular.module('app');
    app.factory('userService', ['$http', 'localizedWebAPIService',
        function ($http, localizedWebAPIService) {

            var factory = {};

            var _getList = function (userName) {
                return $http.get(localizedWebAPIService.get("User/GetList?userName=" + userName)).then(function (results) {
                    return results;
                });
            };
                       
            
            factory.getList = _getList;
            
            return factory;

        }
    ]);
})();