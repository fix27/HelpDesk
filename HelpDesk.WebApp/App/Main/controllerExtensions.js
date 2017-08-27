//расширение контроллера для журнала для педжинга, фильтрации, сортировки
function extendColumnHeader(vm)
{
    //пейджинг-------------------------------------------------------------------------
    vm.totalCount = 0;
    vm.count = 0;
    vm.currentPage = 1;
    vm.pageSize = 20;
    vm.pageChanged = function () {
        vm.refresh();
    };

    //сортировка-----------------------------------------------------------------------
    vm.getOrderArrow = function (propertyName) {
        if (vm.orderInfo.propertyName == propertyName) {
            if (vm.orderInfo.asc)
                return 'glyphicon glyphicon-sort-by-attributes';
            return 'glyphicon glyphicon-sort-by-attributes-alt';
        }
        return null;
    };

    vm.sort = function (propertyName) {
        if (vm.orderInfo.propertyName == propertyName) {
            vm.orderInfo.asc = !vm.orderInfo.asc;
        }
        else {
            vm.orderInfo.propertyName = propertyName;
            vm.orderInfo.asc = true;
        }
        vm.currentPage = 1;
        vm.refresh();
    };

    //фильтрация-----------------------------------------------------------------------
    var p = [];
    for (var propertyName in vm.filter)
    {
        

        p.push(propertyName);
        
        if (Array.isArray(vm.filter[propertyName])) {
            vm['filter' + propertyName] = [];
        }
        else if (vm.filter[propertyName] != null && typeof (vm.filter[propertyName].Value1) !== "undefined") {
            vm['filter' + propertyName] = { Value1: null, Value2: null };
        }
        else {
            vm['filter' + propertyName] = null;
        }
    }

    vm.openFilter = function (propertyName) {

        angular.element("#" + propertyName + "-filter-dialog").modal('show');
        var $modalDialog = angular.element("#" + propertyName + "-filter-dialog .modal-dialog");
        var $body = angular.element("body");
        
        var x = event.clientX;
        var y = event.clientY;
        if (x + $modalDialog.width() > $body.width())
            x = $body.width() - $modalDialog.width() - 20;

        $modalDialog.css("position", "absolute");
        $modalDialog.css("top", y + "px");
        $modalDialog.css("left", x + "px");

        angular.element('.modal-backdrop').removeClass("modal-backdrop");
    };

    vm.isFilter = function (propertyName) {
        

        if (!propertyName) {
            var r = false;
            for (var i = 0; i < p.length; i++)
                r = r || vm.isFilter(p[i]);
            return r;
        }

        if (Array.isArray(vm.filter[propertyName])) {
            return vm.filter[propertyName].length > 0;
        }
        else if (vm.filter[propertyName] != null && typeof (vm.filter[propertyName].Value1) !== "undefined") {
            return vm.filter[propertyName].Value1 != null || vm.filter[propertyName].Value2 != null;
        }
        else {
            return vm.filter[propertyName] != null;
        }
    };

    vm.isNotValidFilter = function (propertyName) {
        if (Array.isArray(vm.filter[propertyName])) {
            for (var i = 0; i < vm["filter" + propertyName].length; i++)
                if (vm["filter" + propertyName][i].Checked)
                    return false;
            return true;
        }
        else if (vm.filter[propertyName] != null && typeof (vm.filter[propertyName].Value1) !== "undefined") {
            return !(vm["filter" + propertyName].Value1 || vm["filter" + propertyName].Value2);
        }
        else {

            return !(vm["filter" + propertyName]);
        }
    };

    vm.applyFilter = function (propertyName) {
        angular.element("#" + propertyName + "-filter-dialog").modal('hide');


        if (Array.isArray(vm.filter[propertyName])) {
            vm.filter[propertyName] = [];
            var list = vm["filter" + propertyName];
            for (var i = 0; i < list.length; i++)
                if (list[i].Checked)
                    vm.filter[propertyName].push(list[i]);
        }
        else if (vm.filter[propertyName] != null && typeof (vm.filter[propertyName].Value1) !== "undefined") {
            vm.filter[propertyName].Value1 = vm["filter" + propertyName].Value1;
            vm.filter[propertyName].Value2 = vm["filter" + propertyName].Value2;
        }
        else {
            vm.filter[propertyName] = vm["filter" + propertyName];

        }

        vm.currentPage = 1;
        vm.refresh();
    };

    vm.resetFilter = function (propertyName, noRefresh) {
        if (!propertyName) {
            for (var i = 0; i < p.length; i++)
                vm.resetFilter(p[i], true);
            vm.refresh();
            return;
        }
        
        angular.element("#" + propertyName + "-filter-dialog").modal('hide');

        if (Array.isArray(vm.filter[propertyName])) {
            var list = vm["filter" + propertyName];
            for (var i = 0; i < list.length; i++)
                list[i].Checked = false;

            vm.filter[propertyName] = [];
        }
        else if (vm.filter[propertyName] != null && typeof (vm.filter[propertyName].Value1) !== "undefined") {
            vm["filter" + propertyName].Value1 = null;
            vm["filter" + propertyName].Value2 = null;
            vm.filter[propertyName].Value1 = null;
            vm.filter[propertyName].Value2 = null;
        }
        else {
            vm["filter" + propertyName] = null;
            vm.filter[propertyName] = null;

        }

        vm.currentPage = 1;
        if (!noRefresh)
            vm.refresh();
    };


   
}