(function () {
    var controllerId = 'app.views.layout';
    angular.module('app').controller(controllerId, [
        '$scope', '$rootScope', function ($scope, $rootScope) {
            var vm = this;
            vm.headerTempleteUrl = '/' + culture + '/AngularTemplate/Header';

            $rootScope.$on('cfpLoadingBar:completed', function () {
                if (vm.loadingFlag) {
                    vm.loadingFlag = false;
                }
            });
        }]);
})();