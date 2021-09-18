Feature: Formatting A Feature in a folder

@cucumber
Scenario: A simple feature in a folder

    Given I have this feature description placed in a folder 'FeatureTest'
        """
        Feature: Clearing Screen
            In order to restart a new set of calculations
            As a math idiot
            I want to be able to clear the screen

        @workflow @slow
        Scenario: Clear the screen
            Given I have entered 50 into the calculator
            And I have entered 70 into the calculator
            When I press C
            Then the screen should be empty
        """
    When I generate the documentation
    Then the JSON file should contain
"""
[
  {
    "keyword": "Feature",
    "name": "Clearing Screen",
    "uri": "FeatureTest/FormattingAFeature.feature",
    "tags": [],
    "line": 1,
    "elements": [
      {
        "keyword": "Scenario",
        "name": "Clear the screen",
        "line": 7,
        "type": "scenario",
        "tags": [
          {
            "name": "@workflow"
          },
          {
            "name": "@slow"
          }
        ],
        "steps": [
          {
            "keyword": "Given",
            "name": "I have entered 50 into the calculator",
            "line": 8,
            "result": {
              "status": "inconclusive",
              "duration": 1
            }
          },
          {
            "keyword": "And",
            "name": "I have entered 70 into the calculator",
            "line": 9,
            "result": {
              "status": "inconclusive",
              "duration": 1
            }
          },
          {
            "keyword": "When",
            "name": "I press C",
            "line": 10,
            "result": {
              "status": "inconclusive",
              "duration": 1
            }
          },
          {
            "keyword": "Then",
            "name": "the screen should be empty",
            "line": 11,
            "result": {
              "status": "inconclusive",
              "duration": 1
            }
          }
"""