(function () {
    var app = angular.module('app');

    var controllerId = 'app.views.shared.fileDelete';
    app.controller(controllerId, [
        '$http', '$scope', 
        function ($http, $scope) {

            var file = $scope.file,
                    state;
            if (file.url) {
                file.$state = function () {
                    return state;
                };
                file.$destroy = function () {
                    state = 'pending';
                    return $http({
                        url: file.deleteUrl,
                        method: file.deleteType
                    }).then(
                        function () {
                            state = 'resolved';
                            $scope.clear(file);
                        },
                        function () {
                            state = 'rejected';
                        }
                    );
                };
            } else if (!file.$cancel && !file._index) {
                file.$cancel = function () {
                    $scope.clear(file);
                };
            }
        }
    ]);
})();