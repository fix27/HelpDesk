(function () {
    var app = angular.module('app');
    app.factory('requestIdService', [
        function () {

            var requestId = 0;
            var factory = {};

            var _set = function (id)
            {
                requestId = id
            }
            var _get = function () {
                return requestId;
            }
            
            factory.get = _get;
            factory.set = _set;
            
            return factory;

        }
    ]);
})();