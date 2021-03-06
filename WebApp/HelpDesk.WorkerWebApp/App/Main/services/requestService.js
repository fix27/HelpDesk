﻿(function () {
    var app = angular.module('app');
    app.factory('requestService', ['$http', 'localizedWebAPIService',
        function ($http, localizedWebAPIService) {

            var factory = {};

            var _getNewByObjectId = function (objectId) {
                return $http.get(localizedWebAPIService.get("Request/GetNewByObjectId?objectId=" + objectId))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getNewByRequestId = function (requestId) {
                return $http.get(localizedWebAPIService.get("Request/GetNewByRequestId?requestId=" + requestId))
                    .then(function (results) {
                        return results;
                    });
            };
            
            var _get = function (requestId) {
                return $http.get(localizedWebAPIService.get("Request/Get?requestId=" + requestId))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getCountRequiresReaction = function () {
                return $http.get(localizedWebAPIService.get("Request/GetCountRequiresReaction"))
                    .then(function (results) {
                        return results;
                    });
            };

            
            
            var _getRequestFilter = function (requestId) {
                return $http.get(localizedWebAPIService.get("Request/GetRequestFilter"))
                    .then(function (results) {
                        return results;
                    });
            };
            
            var _save = function (request) {
                return $http.post(localizedWebAPIService.get("Request/Save"), JSON.stringify(request)).then(function (results) {
                    return results;
                });
            };

            var _createRequestEvent = function (requestEvent) {
                return $http.post(localizedWebAPIService.get("Request/CreateRequestEvent"), JSON.stringify(requestEvent)).then(function (results) {
                    return results;
                });
            };

            var _getList = function (filter, orderInfo, pageInfo) {
                                
                
                var s = (filter.ObjectName != null && filter.ObjectName != 'null') ? "&objectName=" + filter.ObjectName : "";
                s += (filter.DescriptionProblem != null && filter.DescriptionProblem != 'null') ? "&descriptionProblem=" + filter.DescriptionProblem : "";
                s += (filter.WorkerName != null && filter.WorkerName != 'null') ? "&workerName=" + filter.WorkerName : "";
                s += (filter.EmployeeInfo != null && filter.EmployeeInfo != 'null') ? "&employeeInfo=" + filter.EmployeeInfo : "";
                s += (filter.Id != null && filter.Id != 'null') ? "&id=" + filter.Id : "";

                s += "&dateInsert.Value1=" + (filter.DateInsert.Value1 != null ? filter.DateInsert.Value1.toISOString() : "null");
                s += "&dateInsert.Value2=" + (filter.DateInsert.Value2 != null ? filter.DateInsert.Value2.toISOString() : "null");
                s += "&dateEndPlan.Value1=" + (filter.DateEndPlan.Value1 != null ? filter.DateEndPlan.Value1.toISOString() : "null");
                s += "&dateEndPlan.Value2=" + (filter.DateEndPlan.Value2 != null ? filter.DateEndPlan.Value2.toISOString() : "null");
                
                for (var i = 0; i < filter.Statuses.length; i++)
                    s += "&rawStatusIds=" + filter.Statuses[i].Id;

                if (filter.Archive && filter.Archive!="null")
                {
                    s += "&archive=true";
                    s += "&archiveMonth=" + filter.ArchiveMonth;
                    s += "&archiveYear=" + filter.ArchiveYear;
                }
                
                return $http.get(localizedWebAPIService.get("Request/GetList?propertyName=" + orderInfo.propertyName + "&asc=" + orderInfo.asc + "&currentPage=" + pageInfo.currentPage + "&pageSize=" + pageInfo.pageSize + s))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListStatus = function (archive) {

                var url = "Request/GetListStatus";

                if (archive && archive != "null")
                    url += "?archive=true";
                else
                    url += "?archive=false";

                return $http.get(localizedWebAPIService.get(url))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListRequestEvent = function (requestId) {
                
                return $http.get(localizedWebAPIService.get("Request/GetListRequestEvent?requestId=" + requestId))
                    .then(function (results) {
                        return results;
                    });
            };

            var _delete = function (id) {
                return $http.delete(localizedWebAPIService.get("Request/Delete/" + id)).then(function (results) {
                    return results;
                });
            };

            var _getAllowableDeadLine = function (id) {
                return $http.get(localizedWebAPIService.get("Request/GetAllowableDeadLine?requestId="+id))
                    .then(function (results) {
                        return results;
                    });
            };

            var _getListDescriptionProblem = function (name, objectId) {
                return $http.get(localizedWebAPIService.get("Request/GetListDescriptionProblem?name=" + name + "&objectId=" + objectId)).then(function (results) {
                    return results;
                });
            };

            var _getListRequestStateCount = function () {
                return $http.get(localizedWebAPIService.get("Request/GetListRequestStateCount")).then(function (results) {
                    return results;
                });
            };
            
            factory.getNewByObjectId    = _getNewByObjectId;
            factory.getNewByRequestId   = _getNewByRequestId;
            factory.save                = _save;
            factory.createRequestEvent  = _createRequestEvent;
            factory.getCountRequiresReaction = _getCountRequiresReaction;
            factory.getList             = _getList;
            factory.getListStatus       = _getListStatus;
            factory.getRequestFilter    = _getRequestFilter;
            factory.get                 = _get;
            factory.getListRequestEvent = _getListRequestEvent;
            factory.delete              = _delete;
            factory.getAllowableDeadLine = _getAllowableDeadLine;
            factory.getListDescriptionProblem   = _getListDescriptionProblem;
            factory.getListRequestStateCount    = _getListRequestStateCount;

            return factory;

        }
    ]);
})();