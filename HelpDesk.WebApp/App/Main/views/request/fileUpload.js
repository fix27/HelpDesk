(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.request.fileUpload';
    app.controller(controllerId, [
        '$http', '$rootScope', '$scope', 'requestIdService',
        function ($http, $rootScope, $scope, requestIdService) {

            var url = '/api/RequestFileUpload/Upload';
            
            $scope.options = {
                url: url,
                autoUpload: true
            };
            $scope.editMode = true;

            angular.element('#fileupload')
                .bind('fileuploadfail', function (e, data) 
                {
                    var c = data.files.length;
                    $scope.queue.splice($scope.queue.length - c, c)
                    $rootScope.$broadcast('fileUploadFailEvent', { errorMsg: data.jqXHR.responseJSON.Message });
                })
                

            $scope.loadingFiles = true;

            var getList = function ()
            {
                $http.get(url + "?forignKeyId=" + requestIdService.get())
                .then(
                    function (response) {
                        $scope.loadingFiles = false;
                        $scope.queue = response.data.files || [];
                    },
                    function () {
                        $scope.loadingFiles = false;
                    }
                );
            }

            getList();

            $scope.$on('createNewRequestEvent', function (e) {
                $scope.editMode = true;
                getList();
            });

            $scope.$on('editModeEvent', function (e, data) {
                $scope.editMode = data.editMode;
            });

        }
    ]);
})();