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

            $http.post('api/trivia/post', { 'questionId': option.questionId, 'optionId': option.id })
                .success(function (data, status, headers, config) {
                    $scope.correctAnswer = data;
                    $scope.working = false;
                })

                .error(function (data, status, headers, config) {
                    $scope.title = "Oops... something went wrong";
                    $scope.working = false;
            });
        };
    })
    .controller('QuizQuestions', function ($scope, $http) {
        $scope.questions = [];

        $scope.getQuestions = function () {
            $http.get("../api/TriviaQuestionsApi/GetTriviaQuestions")
                .success(function (data, status, headers, config) {
                    $scope.questions = data;
                })

                .error(function (data, status, headers, config) {
                    alert('Oops... something went wrong');
            });
        };
    });