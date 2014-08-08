$(function () {
	$('.container-header').data('size', 'big');
});

$(window).scroll(function () {
	if ($(document).scrollTop() > 0) {
		if ($('.container-header').data('size') == 'big') {
			$('.header-text').css('font-size', '2em');
			$('.header-text').css('padding-top', '0px');
			$('.container-header').data('size', 'small');
			$('.container-header').stop().animate({
				height: '40px'
			}, 600);

		}
	}
	else {
		if ($('.container-header').data('size') == 'small') {
			$('.header-text').css('font-size', '3em');
			$('.header-text').css('padding-top', '25px');
			$('.container-header').data('size', 'big');
			$('.container-header').stop().animate({
				height: '100px'
			}, 600);
		}
	}
});

$('#main-todo-board').draggable();

$('.header-text').click(function () {
	$('.todo-body').slideToggle();
});

//$('.drop-wrapper > li >ul > li').click(function () {
//	$('.drop-wrapper > li').text($('.drop-wrapper > li >ul > li').text())
//});

//angular
angular.module('ToDoTest',[]).
controller('ToDoCtrl', ['$scope', function($scope) {
    $scope.todos = [
        {'title':'practice angular', 'done':false}
    ];

    $scope.addTodo = function () {
        $scope.todos.push({ 'title': $scope.newTodo, 'done': false });
        $scope.newTodo = "";
    }

    $scope.clearCompleted = function () {
        $scope.todos = $scope.todos.filter(function (object) {
            return !object.done
        });
    }

    $scope.todoRemaining = function () {
        var count = 0;
        angular.forEach($scope.todos, function (todo) {
            count += todo.done ? 0 : 1;
        });
        return count;
    }

}])