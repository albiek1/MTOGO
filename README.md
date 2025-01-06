# MTOGO

# Testing Plan for MTOGO System

## Scope
We will be doing unit testing for individual modules within the MTOGO system.  
Integration testing will be done through our service interactions, e.g., the restaurant and delivery.  
System testing will be done to ensure the system works overall.  
Acceptance testing will be used for requirements and workflow handling.

## Test Goals
We want to test the key functionalities - our C.R.U.D methods. Our aim is to test all of the system's functionalities.  
Our goals for the tests include:
- Validating that the functionality of the application behaves based on the specification.
- Confirming that the different components within the system are interacting as expected.

## Sprint Timelines
The plan is to follow a test schedule that is as follows:

| Test               | Date range           |
|--------------------|----------------------|
| Unit Testing       | 20/12/24 - 4/1/25    |
| Integration Testing| 20/12/24 - 4/1/25    |
| System Testing     | 20/12/24 - 4/1/25    |
| Acceptance Testing | 20/12/24 - 4/1/25    |

## Test Approach
- We will always manually run all the tests and make sure they pass before committing.
- Our tests will follow the **Arrange-Act-Assert** style.
- Duplication of tests is okay if it is beneficial.

## Test Types
Our testing types consist of the following:

### Unit Tests
- **Where to use**: For each of the components making up the core functionalities.
- **Dependencies**: Minimal. We will likely use test doubles in some tests to isolate the component during the tests.
- **Criticality**: Highest priority in this specific test scenario as we want to ensure that the basic functionality functions as expected.

### Integration Tests
- **Where to use**: In the areas where multiple components interact.
- **Criticality**: Second-highest priority as we want to make sure that the components interact with each other as expected.

### System Testing
- **Focus**: The entire system’s functionality.
- **End-to-end testing** of food ordering and delivery.
- **Tools**: Selenium will be used for UI testing.
- **Criticality**: Lowest priority during development, as we will be building our backend first.

### Acceptance Testing
- **Focus**: Handling the order workflows.
- We’ll conduct these tests by simulating user scenarios.
- **Criticality**: Third in our hierarchy, as the flow of the system needs to work if we are to succeed in simulating a real order.

## Roles and Responsibilities
### What are the different roles?
- **Testers, Test Managers, and Developers**: All roles are fused into one, as we are a very small team of 2 people. We will all be responsible for performing the actual test cases and making sure that the software complies with the quality requirements, as well as planning the test cases.
- From a development perspective, we will be responsible for correcting the mistakes within the software that we might find during tests.

## Testing Tools
The tests will be made using the following tools:
- **xUnit** for unit testing.
- **Selenium** for UI testing.
