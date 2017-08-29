(function () {
    var app = angular.module('app');
    app.factory('employeeObjectService', ['$http', 'localizedWebAPIService',
        function ($http, localizedWebAPIService) {

            var factory = {};

            var _getNewRequestObjectIS = function () {
                return $http.get(localizedWebAPIService.get("EmployeeObject/GetNewRequestObjectIS"))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getNewRequestObjectTO = function () {
                return $http.get(localizedWebAPIService.get("EmployeeObject/GetNewRequestObjectTO"))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListEmployeeObject = function (filter, orderInfo, pageInfo) {

                var s = (filter.ObjectName!=null && filter.ObjectName!='null')? "&objectName=" + filter.ObjectName: "";
                for (var i = 0; i < filter.Wares.length; i++)
                    s += "&wares=" + filter.Wares[i].Id;

                return $http.get(localizedWebAPIService.get("EmployeeObject/GetListEmployeeObject?propertyName=" + orderInfo.propertyName + "&asc=" + orderInfo.asc + "&currentPage=" + pageInfo.currentPage + "&pageSize=" + pageInfo.pageSize + s))
                    .then(function (results) {
                        return results;
                    });
            };

            

            var _getListEmployeeObjectByName = function (objectName) {

                var url = "EmployeeObject/GetListEmployeeObjectByName";
                if (objectName)
                    url += "?objectName=" + objectName;
                
                return $http.get(localizedWebAPIService.get(url))
                    .then(function (results) {
                        return results;
                    });
            };

                        
            var _getListWare = function () {
                return $http.get(localizedWebAPIService.get("EmployeeObject/GetListWare"))
                    .then(function (results) {
                        return results;
                    });
            };
            //----  
            var _getListAllowableObjectIS = function (name) {
                var url = "EmployeeObject/GetListAllowableObjectIS";
                if (name)
                    url += "?name=" + name;

                return $http.get(localizedWebAPIService.get(url))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListAllowableObjectType = function () {
                return $http.get(localizedWebAPIService.get("EmployeeObject/GetListAllowableObjectType"))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListHardType = function (name) {
                return $http.get(localizedWebAPIService.get("EmployeeObject/GetListHardType?name=" + name))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListModel = function (manufacturerId, name) {
                return $http.get(localizedWebAPIService.get("EmployeeObject/GetListModel?manufacturerId=" + manufacturerId + "&name=" + name))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListManufacturer = function (name) {
                return $http.get(localizedWebAPIService.get("EmployeeObject/GetListManufacturer?name=" + name))
                    .then(function (results) {
                        return results;
                    });
            };

            var _addIS = function (RequestObjectIS) {
                return $http.post(localizedWebAPIService.get("EmployeeObject/AddIS"), JSON.stringify(RequestObjectIS)).then(function (results) {
                    return results;
                });
            };

            var _addTO = function (RequestObjectTO) {
                return $http.post(localizedWebAPIService.get("EmployeeObject/AddTO"), JSON.stringify(RequestObjectTO)).then(function (results) {
                    return results;
                });
            };

            var _delete = function (id) {
                return $http.delete(localizedWebAPIService.get("EmployeeObject/Delete/"+id)).then(function (results) {
                    return results;
                });
            };

            factory.getNewRequestObjectIS       = _getNewRequestObjectIS;
            factory.getNewRequestObjectTO       = _getNewRequestObjectTO;
            factory.getListEmployeeObject       = _getListEmployeeObject;
            
            factory.getListEmployeeObjectByName = _getListEmployeeObjectByName;
            factory.getListWare                 = _getListWare;

            factory.getListAllowableObjectIS    = _getListAllowableObjectIS;
            factory.getListAllowableObjectType  = _getListAllowableObjectType;
            factory.getListHardType             = _getListHardType;
            factory.getListModel                = _getListModel;
            factory.getListManufacturer         = _getListManufacturer;
            factory.addIS                       = _addIS;
            factory.addTO                       = _addTO;
            factory.delete                      = _delete;

            return factory;

        }
    ]);
})();