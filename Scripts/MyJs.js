(function (angular) {
    'use strict';

    // Define app and elements
    angular.module("flexbox", ['ngSanitize', 'ui.bootstrap'])
        // Flexbox controller (called from the component)
        .controller('FlexboxCtrl', FlexboxCtrl);

    // Dependencies injections
    FlexboxCtrl.$inject = ['$uibModal', '$document'];

    // Flexbox controller (hoists)
    function FlexboxCtrl($uibModal, $document) {
        var $ctrl = this,
            Element = El;

        $ctrl.minHeight = 707;

        $ctrl.maxWidth = 1920;

        $ctrl.infiniteItems = false;

        /**
         * Wrapper config
         */

        // Display
        $ctrl.display = 'flex';
        // Direction
        $ctrl.direction = 'row';
        // Wrap
        $ctrl.wrap = 'nowrap';
        // Justify content
        $ctrl.justifyContent = 'flex-start';
        // Align items
        $ctrl.alignItems = 'stretch';
        // Align content
        $ctrl.alignContent = 'stretch';


        /**
         * Wrapper children (elements)
         */
        $ctrl.popover = {
            templateUrl: 'popover.html',
            title: 'Element config'
        };

        $ctrl.elements = [
            new Element(),
            new Element(),
            new Element(),
            new Element(),
            new Element()
        ];

        $ctrl.removeElement = removeElement;
        $ctrl.addElement = addElement;
        $ctrl.editElement = editElement;

        function El() {
            this.alignOptions = [
                { id: 0, name: "none" },
                { id: 1, name: "flex-start" },
                { id: 2, name: "flex-end" },
                { id: 3, name: "center" },
                { id: 4, name: "stretch" },
                { id: 5, name: "baseline" }
            ];

            this.grow = 0;
            this.shrink = 1;
            this.basis = '500px';
            this.height = 'auto';
            this.order = 0;
            this.alignSelf = this.alignOptions[0];
        }

        function removeElement(index) {
            $ctrl.elements.splice(index, 1);
            console.log($ctrl.elements.length);
        }

        function addElement() {
            console.log($ctrl.elements);
            $ctrl.elements.push(new Element());
        }

        function editElement(el, index) {
            $uibModal.open({
                animation: true,
                templateUrl: 'modal.html',
                size: 'sm',
                controllerAs: '$ctrl',
                controller: function () {
                    console.log(el);
                }
            });
        }

    }

    // Run app
    angular.bootstrap(document, ['flexbox']);
})(angular)