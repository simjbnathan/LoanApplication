# LoanApp

## Description

LoanApp is a web application designed to streamline the loan application process. It allows users to apply for loans online, calculate repayment amounts, and receive instant quotes. The application offers various loan products with different terms and interest rates to suit the needs of different borrowers.

## Features

- Consume API to create LoanRequest. It will then redirect to loan application page.
- Calculate repayment amounts based on loan amount, term, and selected product
- Choose from multiple loan products with different interest rates and terms
- Validate loan applications to ensure eligibility and compliance with regulations
- Store and manage loan application data securely

## Installation

To run LoanApp locally, follow these steps:

1. Clone the repository: `[git clone https://github.com/simjbnathan/LoanApplication.git]`
2. Open LoanAppApi and LoanWebApp projects.
3. Run both projects.

## Usage

1. Visit the LoanApp API, Run this request to redirect to the main page.
    POST https://localhost:7120/api/LoanApplication/LoanRequest
    Request Body:
   {
    "AmountRequired": 5000,
    "Term": 2,
    "Title": "Mr.",
    "FirstName": "Jonathan",
    "LastName": "Zerda",
    "DateOfBirth": "1980-01-01",
    "Mobile": "0422111333",
    "Email": "jonathanzerda@gmail.com"
   }

3. Redirect Url will redirect you to LoanApp
4. Fill out the loan application form with your personal details and loan preferences.
5. Click on "Calculate Quote" to see the estimated repayment amount.
6. Review the quote and click on "Apply Now" to submit your application.
7. Wait for the application to be processed and receive confirmation.

## Contributing

Contributions to LoanApp are welcome! To contribute:

1. Fork the repository
2. Create a new branch: `git checkout -b feature/my-feature`
3. Commit your changes: `git commit -am 'Add new feature'`
4. Push to the branch: `git push origin feature/my-feature`
5. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Credits

- Developed by [simjbnathan](https://github.com/simjbnathan)
- Inspired by [Similar Project](https://github.com/money)

## Contact

For questions or support, contact us at simjbnathan@gmail.com.
