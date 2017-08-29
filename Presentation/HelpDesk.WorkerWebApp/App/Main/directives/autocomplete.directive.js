var ngAutocomplete = angular.module('autocomplete.directive', []);
ngAutocomplete.directive('autocomplete', function () {
    return {
        restrict: 'A',
        scope: { select: '=', search: '=' },
        link: function (scope, element, attrs) {
            

            element.autocomplete({
                source: function (request, response) {
                    scope.search(request.term, response);
                },
                minLength: 2,
                select: function (event, selectedItem) {

                    scope.select(selectedItem.item.item);
                    scope.$apply();
                }
            });


        }
    }
});