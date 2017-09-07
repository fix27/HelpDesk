(function () {
    var app = angular.module('app');
    app.factory('employeeService', ['$http', 'localizedWebAPIService',
        function ($http, localizedWebAPIService) {

            var factory = {};

            
            var _get = function () {
                return $http.get(localizedWebAPIService.get("Employee/Get")).then(function (results) {
                    return results;
                });
            };

            var _getListPost = function (name) {
                return $http.get(localizedWebAPIService.get("Employee/GetListPost?name=" + name)).then(function (results) {
                    return results;
                });
            };

            var _getListOrganization = function (name) {
                return $http.get(localizedWebAPIService.get("Employee/GetListOrganization?name=" + name)).then(function (results) {
                    return results;
                });
            };

            var _getListEmployeeByName = function (name) {
                return $http.get(localizedWebAPIService.get("Employee/GetListEmployee?name=" + name)).then(function (results) {
                    return results;
                });
            };

                       
            var _save = function (employee) {
                return $http.post(localizedWebAPIService.get("Employee/Save"), JSON.stringify(employee)).then(function (results) {
                    return results;
                });
            };

           
            factory.save        = _save;
            factory.get         = _get;
            factory.getListPost = _getListPost;
            factory.getListOrganization     = _getListOrganization;
            factory.getListEmployeeByName   = _getListEmployeeByName;

            return factory;

        }
    ]);
})();