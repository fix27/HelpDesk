(function () {
    
    var controllerId = 'app.views.layout.header';
    angular.module('app').controller(controllerId, [
        '$rootScope', '$state', '$location', '$interval', 'localizedMenuService', 'requestService',
        function ($rootScope, $state, $location, $interval, localizedMenuService, requestService) {

            
            var vm = this;
            vm.errorMsg = null;

            vm.requestStateCounts = [];
            vm.menu = {};
            localizedMenu = localizedMenuService.get();
            vm.menu.items = [];
            for (var propertyName in localizedMenu) {
                vm.menu.items.push({ displayName: localizedMenu[propertyName], name: propertyName })
            }
                        
            vm.isActive = function(item) {
                return (item.name == $state.current.name);
            };

            vm.go = function(stateName) {
                localStorage.setItem('stateName', stateName);
                $state.go(stateName);
            };
           
            $rootScope.$on('employeeChangedSuccess', function (event, data) {
                angular.element("#span-fio").text(data.fio);
            });

            $rootScope.$on('error', function (event, data) {
                vm.errorMsg = data.errorMsg;
            });
            
            //чтобы при перезагрузке страницы возвращаться в текущее меню
            var stateName = localStorage.getItem('stateName');
            
            if (stateName && localizedMenu[stateName])
                vm.go(stateName);
            else
                vm.go('requestHistory');

            var getStatistics = function ()
            {
                requestService.getCountRequiresReaction().then(function (results) {
                    vm.menu.items[vm.menu.items.length == 1 ? 1 : 0].countRequiresConfirmation = results.data.data;
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data? error.data.Message: "Нет связи с сервером" });
                });
            }
            
            $interval(getStatistics, 5000);
        }
    ]);
})();