(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.personalProfile.profile';
    app.controller(controllerId, [
        '$rootScope', 'personalProfileService', 'modalService',
        function ($rootScope, personalProfileService, modalService) {

            var vm = this;

            vm.personalProfile = {};
            vm.errors = {};
             
            vm.loadingFlag = false;
            $rootScope.$on('cfpLoadingBar:completed', function () {
                if (vm.loadingFlag) {
                    vm.loadingFlag = false;
                }
            });

            vm.invalidForm = function () {
                return !(vm.personalProfile.FM && vm.personalProfile.IM && vm.personalProfile.OT);
            };

            
            vm.save = function () {
                vm.loadingFlag = true;
                personalProfileService.save(vm.personalProfile).then(function (results) {
                    
                    vm.errors = {};
                    
                    if (results.data.success === false) {
                        vm.errors = results.data.errors;
                    }
                    else {
                        init();
                        $rootScope.$broadcast("personalProfileChangedSuccess", { fio: vm.personalProfile.FM + " " + vm.personalProfile.IM + " " + vm.personalProfile.OT });
                    }
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            var personalProfile = {};
            var init = function () {
                personalProfileService.get().then(function (results) {
                    vm.personalProfile = results.data.data;
                    
                    if (vm.personalProfile.OrganizationName && vm.personalProfile.OrganizationName != 'null')
                        vm.personalProfile.OrganizationName += (", " + vm.personalProfile.OrganizationAddress);

                    angular.copy(vm.personalProfile, personalProfile);
                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

                vm.errors = {};
            }

            vm.hasChange = function ()
            {
                return !angular.equals(personalProfile, vm.personalProfile);
            }

            vm.getListPost = function (name, response) {

                if (name == 'undefined' || name == 'null' || name == null)
                    return;

                personalProfileService.getListPost(name).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.Name, value: t.Name, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };
            
            vm.selectPost = function (p) {
                vm.personalProfile.PostId = p.Id;
            };

            vm.getListOrganization = function (name, response) {

                if (name == 'undefined' || name == 'null' || name == null)
                    return;

                personalProfileService.getListOrganization(name).then(function (results) {
                    response($.map(results.data.data, function (t) {
                        return { label: t.Name + ", " + t.Address, value: t.Name + ", " + t.Address, item: t };
                    }));

                }, function (error) {
                    $rootScope.$broadcast("error", { errorMsg: error.data.Message });
                });

            };

            vm.selectOrganization = function (o) {
                vm.personalProfile.OrganizationId = o.Id;
            };

            vm.openOrganizationDictionary = function ()
            {
                modalService.showModal({
                    templateUrl: "/AngularTemplate/Organizations",
                    controller: "app.views.organization.list as vm",
                    inputs: {
                        params: { }
                    }
                }).then(function (modal) {
                    modal.element.modal();
                    modal.close.then(function (result) {

                        if (result.cancel)
                            return;
                        vm.personalProfile.OrganizationId = result.organization.Id;
                        vm.personalProfile.OrganizationName = result.organization.Name;

                    });
                });
            }
                        
            vm.cancel = function ()
            {
                init();
            }

            init();
     
        }
    ]);
})();