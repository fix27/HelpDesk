(function () {
    var app = angular.module('app');
    app.factory('requestProfileService', ['$http', 'localizedWebAPIService',
        function ($http, localizedWebAPIService) {

            var factory = {};

            var _getNewRequestObjectIS = function () {
                return $http.get(localizedWebAPIService.get("RequestProfile/GetNewRequestObjectIS"))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getNewRequestObjectTO = function () {
                return $http.get(localizedWebAPIService.get("RequestProfile/GetNewRequestObjectTO"))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListPersonalObject = function (filter, orderInfo, pageInfo) {

                var s = (filter.ObjectName!=null && filter.ObjectName!='null')? "&objectName=" + filter.ObjectName: "";
                for (var i = 0; i < filter.Wares.length; i++)
                    s += "&wares=" + filter.Wares[i].Id;

                return $http.get(localizedWebAPIService.get("RequestProfile/GetListPersonalObject?propertyName=" + orderInfo.propertyName + "&asc=" + orderInfo.asc + "&currentPage=" + pageInfo.currentPage + "&pageSize=" + pageInfo.pageSize + s))
                    .then(function (results) {
                        return results;
                    });
            };

            

            var _getListPersonalObjectByName = function (objectName) {

                var url = "RequestProfile/GetListPersonalObjectByName";
                if (objectName)
                    url += "?objectName=" + objectName;
                
                return $http.get(localizedWebAPIService.get(url))
                    .then(function (results) {
                        return results;
                    });
            };

                        
            var _getListWare = function () {
                return $http.get(localizedWebAPIService.get("RequestProfile/GetListWare"))
                    .then(function (results) {
                        return results;
                    });
            };
            //----  
            var _getListAllowableObjectIS = function (name) {
                var url = "RequestProfile/GetListAllowableObjectIS";
                if (name)
                    url += "?name=" + name;

                return $http.get(localizedWebAPIService.get(url))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListAllowableObjectType = function () {
                return $http.get(localizedWebAPIService.get("RequestProfile/GetListAllowableObjectType"))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListHardType = function (name) {
                return $http.get(localizedWebAPIService.get("RequestProfile/GetListHardType?name=" + name))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListModel = function (manufacturerId, name) {
                return $http.get(localizedWebAPIService.get("RequestProfile/GetListModel?manufacturerId=" + manufacturerId + "&name=" + name))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListManufacturer = function (name) {
                return $http.get(localizedWebAPIService.get("RequestProfile/GetListManufacturer?name=" + name))
                    .then(function (results) {
                        return results;
                    });
            };

            var _addIS = function (RequestObjectIS) {
                return $http.post(localizedWebAPIService.get("RequestProfile/AddIS"), JSON.stringify(RequestObjectIS)).then(function (results) {
                    return results;
                });
            };

            var _addTO = function (RequestObjectTO) {
                return $http.post(localizedWebAPIService.get("RequestProfile/AddTO"), JSON.stringify(RequestObjectTO)).then(function (results) {
                    return results;
                });
            };

            var _delete = function (id) {
                return $http.delete(localizedWebAPIService.get("RequestProfile/Delete/"+id)).then(function (results) {
                    return results;
                });
            };

            factory.getNewRequestObjectIS           = _getNewRequestObjectIS;
            factory.getNewRequestObjectTO           = _getNewRequestObjectTO;
            factory.getListPersonalObject       = _getListPersonalObject;
            
            factory.getListPersonalObjectByName = _getListPersonalObjectByName;
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