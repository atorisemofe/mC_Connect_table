# Overview
This application is designed to streamline the operation of a restaurant by managing tables, orders, payments, and customer interactions. It integrates with the Star mC-Connect Table (MCTs) to facilitate real-time communication between customers, the front-of-house staff, and the kitchen. The app is divided into the following sections:

- Front of House: Manages table states and notifications.
- Back of House: Handles order statuses and communication with the kitchen.
- Menu: Displays menu options via QR codes sent to MCT devices.
- Payments: Allows customers to pay via QR codes.
- Survey: Presents a customer survey after payment.

## mC-Connect Table API Documentation and Guide
https://star-m.jp/products/s_print/sdk/mCollection/mC-Connect-Table/SoftwareDevelopmentGuide/en/web-api/index.html

## Features
### 1. Front of House (Table and Layout Management)
- Table States: Each table can be in either an available or occupied state. When a customer is seated, the table is marked as occupied.
- MCT Device Integration: Each table is assigned an MCT ID and a table number. When a table is created, three images (for help, menu, and payment actions) are pre-stored in the MCT device using the https://mc-connect-manager.smcs.io/api/v1/registration/image/push-switch API. This allows the MCT to respond more quickly to button presses (3-4 seconds instead of 8-10 seconds). The images correspond to the following actions:

  - 1 Press: Requests help from the waiter.
  - 2 Press: Generates a QR code linking to the menu.
  - 3 Press: Generates a QR code linking to the payment page.

### Webhook and Notification Workflow
- Help Request (1 Press):
  - When the customer presses the button once, the MCT sends a webhook to the app.
  - The MCT displays the pre-stored image for help requested/call waiter

![server](https://github.com/user-attachments/assets/afcf2c79-44d6-4aeb-b95a-b3f923fbac4a)

  - The webhook triggers a hub notification that is sent to the front-of-house staff, displaying "Help Requested" in red on the specific table assigned to that MCT.
  - After 15 seconds, the instruction image is resent to the MCT device.
   -API Used to Send Image: The instruction image is sent to the MCT using the https://mc-connect-manager.smcs.io/api/v1/update-image API.
- Menu QR Code (2 Press):
  - When the customer presses the button twice, the MCT send a webhook to the app.
  - The MCT displays the pre-stored image for generating a Menu QR code

![QR1](https://github.com/user-attachments/assets/5a0ee8b0-4443-43ec-959d-d0ff09a0a9da)

  - A menu URL is generated with the following parameters: table number, MCT ID, and session ID.
  - A QR code is generated from the URL using the template parameter of the https://mc-connect-manager.smcs.io/api/v1/update-image API, which automatically embeds the URL in the image.
  - The QR code image is then sent to the MCT for the customer to scan and view the menu.
  - After 30 seconds, the instruction image is resent to the MCT.
  - API Used to Send Image: The QR code image for the menu is sent to the MCT using the https://mc-connect-manager.smcs.io/api/v1/update-image API, with the template parameter set to generate the QR code.
- Payment QR Code (3 Press):
  - When the customer presses the button three times, the MCT sends a webhook to the app.
  - The MCT displays the pre-stored image for generating a Payment QR code.
  
![output-onlinepngtools (16)](https://github.com/user-attachments/assets/c8155dee-c9cc-4441-8515-3e28ee51cd13)

  - A payment URL is generated with similar parameters (table number, MCT ID, and session ID).
  - A QR code is generated from the payment URL using the template parameter of the https://mc-connect-manager.smcs.io/api/v1/update-image API, which automatically embeds the payment URL in the QR code image.
  - The QR code is then sent to the MCT for the customer to scan and complete the payment.
  - API Used to Send Image: The QR code image for the payment is sent using the https://mc-connect-manager.smcs.io/api/v1/update-image API, with the template parameter used to generate the QR code.

### 2. Back of House (Order and Kitchen Management)
- Order Workflow: Once the customer places an order, the details are sent to the kitchen.

  - Order States: Orders can be marked as New, Preparing, Ready, or Done.
    - New is the default state.
  - When the order status is updated to Preparing, an image is sent to the MCT notifying the customer that the kitchen is preparing the order.

![orderPrepare-min](https://github.com/user-attachments/assets/3c4b3934-6fb7-42cc-bb7f-4b22cba3385f)

  - When the order is marked as Ready, an image is sent to the MCT informing the customer that their order will be out shortly. Simultaneously, a hub notification is sent to the front-of-house staff to alert them that the order is ready for delivery.

![orderReady](https://github.com/user-attachments/assets/bbebd26b-80bc-4f03-91bc-5c610f41f200)

  - API Used to Send Image: The ready order image is sent using https://mc-connect-manager.smcs.io/api/v1/update-image.
  - Final Order State: Once the order is delivered to the customer, it is marked as Done in the database.

### 3. Menu (QR Code Menu)
- When a customer presses the MCT button twice (2 Press), a unique URL linking to the menu is generated.
- A QR code is created using the template parameter of the https://mc-connect-manager.smcs.io/api/v1/update-image API, which automatically embeds the URL in the QR code image.
- The QR code is sent to the MCT device for the customer to scan and view the menu.
- API Used to Send Image: The QR code is sent using https://mc-connect-manager.smcs.io/api/v1/update-image, with the template parameter used to generate the QR code.

### 4. Payments
- When a customer presses the MCT button three times (3 Press), a URL is generated for payment.
- The payment URL is embedded in a QR code, and the QR code is sent to the MCT for the customer to scan.
- The QR code is generated using the template parameter of the https://mc-connect-manager.smcs.io/api/v1/update-image API, which automatically embeds the payment URL into the QR code image.
- The customer is then presented with a list of orders for that session that have not yet been paid for and can complete the payment process.
- API Used to Send Image: The payment QR code is sent using https://mc-connect-manager.smcs.io/api/v1/update-image, with the template parameter used to generate the QR code.

### 5. Survey
- After the customer completes the payment, they are redirected to a survey page where they can provide feedback about their dining experience.
