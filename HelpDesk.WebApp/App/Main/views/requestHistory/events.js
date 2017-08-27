(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.requestHistory.events';
    app.controller(controllerId, [
                 '$rootScope', 'requestService', 'params',
        function ($rootScope, requestService, params) {

            var vm = this;

            requestService.getListRequestEvent(parseInt(params.requestId)).then(function (results) {
                vm.recs = results.data.data;                
            }, function (error) {
                $rootScope.$broadcast("error", { errorMsg: error.data.Message });
            });

        }

    ]);
})();