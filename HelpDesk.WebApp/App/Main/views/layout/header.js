(function () {
    
    var controllerId = 'app.views.layout.header';
    angular.module('app').controller(controllerId, [
        '$rootScope', '$state', '$location', '$interval', 'localizedMenuService', 'requestService',
        function ($rootScope, $state, $location, $interval, localizedMenuService, requestService) {

            
            var vm = this;
            vm.errorMsg = null;

           
            vm.menu = {};
            localizedMenu = localizedMenuService.get();
            vm.menu.items = [
                { displayName: localizedMenu.request,           name: 'request' },
                { displayName: localizedMenu.requestHistory,    name: 'requestHistory' },
                { displayName: localizedMenu.requestProfile,    name: 'requestProfile' },
                { displayName: localizedMenu.personalProfile,   name: 'personalProfile' }
            ];
            
            vm.isActive = function(item) {
                return (item.name == $state.current.name);
            };

            vm.go = function(stateName) {
                localStorage.setItem('stateName', stateName);
                $state.go(stateName);
            };
           
            $rootScope.$on('personalProfileChangedSuccess', function (event, data) {
                angular.element("#span-fio").text(data.fio);
            });

            $rootScope.$on('error', function (event, data) {
                vm.errorMsg = data.errorMsg;
            });
            
            //чтобы при перезагрузке страницы возвращаться в текущее меню
            var stateName = localStorage.getItem('stateName');
            if (stateName)
                vm.go(stateName);

            var getCountRequiresConfirmation = function()
            {
                requestService.getCountRequiresConfirmation().then(function (results) {
                    vm.menu.items[1].countRequiresConfirmation = results.data.data;
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data? error.data.Message: "Нет связи с сервером" });
                });
            }
            
            $interval(getCountRequiresConfirmation, 5000);
        }
    ]);
})();