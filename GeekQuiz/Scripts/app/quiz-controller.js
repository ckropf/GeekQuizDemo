angular.module('QuizApp', [])
    .controller('QuizCtrl', function ($scope, $http) {
        $scope.answered = false;
        $scope.title = "loading question...";
        $scope.options = [];
        $scope.correctAnswer = false;
        $scope.working = false;

        $scope.answer = function () {
            return $scope.correctAnswer ? 'correct' : 'incorrect';
        };

        $scope.nextQuestion = function () {
            $scope.working = true;

            $scope.answered = false;
            $scope.title = "loading question...";
            $scope.options = [];

            $http.get("api/trivia").success(function (data, status, headers, config) {
                $scope.options = data.options;
                $scope.title = data.title;
                $scope.answered = false;
                $scope.working = false;
            }).error(function (data, status, headers, config) {
                $scope.title = "Oops... something went wrong";
                $scope.working = false;
            });
        };

        $scope.sendAnswer = function (option) {
            $scope.working = true;
            $scope.answered = true;

            $http.post('api/trivia/post', { 'questionId': option.questionId, 'optionId': option.id }).success(function (data, status, headers, config) {
                $scope.correctAnswer = data;
                //NOTE: I'm not sure why, but I had to change the line above from the original: $scope.correctAnswer = (data === "true");
                //In the demo app it is working, but here $scope.correctAnswer would always be set to false because data was coming back as boolean, not string,
                //so (data === "true") would evaluate to false, because data == true, not "true". However, in apparently the EXACT SAME CONDITIONS, the demo
                //app would work fine. I spent too much time trying to figure it out and failed anyway...

                $scope.working = false;
            }).error(function (data, status, headers, config) {
                $scope.title = "Oops... something went wrong";
                $scope.working = false;
            });
        };
    })
    .controller('QuizQuestions', function ($scope, $http) {
        $scope.questions = [];

        $scope.getQuestions = function () {
            $http.get("../api/TriviaQuestionsApi/GetTriviaQuestions").success(function (data, status, headers, config) {
                $scope.questions = data;
            }).error(function (data, status, headers, config) {
                alert('Oops... something went wrong');
            });
        };
    });