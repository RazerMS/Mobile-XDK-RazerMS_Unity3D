<!--
# license: Copyright © 2011-2016 MOLPay Sdn Bhd. All Rights Reserved. 
-->

# molpay-mobile-xdk-unity3d

This is the complete and functional MOLPay Unity payment module that is ready to be implemented into Unity project through Unity C# script copy and paste. An example application project (MOLPayXDKExample) is provided for MOLPayXDK Unity integration reference.

This plugin provides an integrated MOLPay payment module that contains a wrapper 'MOLPay.cs' and an upgradable core as the 'molpay-mobile-xdk-www' folder, which the latter can be separately downloaded at https://github.com/MOLPay/molpay-mobile-xdk-www and update the local version.

## Recommended configurations

    - Unity Version: 5.3.4f1 ++

    - UniWebView 2 ++

    - Unity JSON Object

    - Minimum Android target version: Android 4.4

    - Minimum iOS target version: 7.0

## Installation

    Step 1 - Copy and paste MOLPay.cs into the Assets folder of your Unity project

    Step 2 - Copy and paste molpay-mobile-xdk-www folder (can be separately downloaded at https://github.com/MOLPay/molpay-mobile-xdk-www) into Assets\StreamingAssets\ folder of your Unity project

    Step 3 - Purchase UniWebView from http://uniwebview.onevcat.com/. After purchasing you should be able to download a Unity package file (e.g. uniwebview_2_7_1.unitypackage). Double click on the file to import it into your unity project

    Step 4 - Open JSON Object in Unity Asset Store. You can open it from https://www.assetstore.unity3d.com/en/#!/content/710 by clicking the "Open in Unity" button. After that import it into your unity project

    Step 5 - Add the result callback function
    public void MolpayCallback (string transactionResult)
    {
        Debug.Log("MolpayCallback transactionResult = " + transactionResult);
    }

## Using MOLPayXDK namespace

    using MOLPayXDK;

## Instantiate the Molpay object

    MOLPay molpay = new MOLPay();

## Prepare the Payment detail object

    Dictionary<String, object> paymentDetails = new Dictionary<String, object>();
    // Mandatory String. A value more than '1.00'
    paymentDetails.Add(MOLPay.mp_amount, "");

    // Mandatory String. Values obtained from MOLPay
    paymentDetails.Add(MOLPay.mp_username, "");
    paymentDetails.Add(MOLPay.mp_password, "");
    paymentDetails.Add(MOLPay.mp_merchant_ID, "");
    paymentDetails.Add(MOLPay.mp_app_name, "");
    paymentDetails.Add(MOLPay.mp_verification_key, "");

    // Mandatory String. Payment values
    paymentDetails.Add(MOLPay.mp_order_ID, "");
    paymentDetails.Add(MOLPay.mp_currency, "MYR");
    paymentDetails.Add(MOLPay.mp_country, "MY");
    
    // Optional String.
    paymentDetails.Add(MOLPay.mp_channel, ""); // Use 'multi' for all available channels option. For individual channel seletion, please refer to "Channel Parameter" in "Channel Lists" in the MOLPay API Spec for Merchant pdf. 
    paymentDetails.Add(MOLPay.mp_bill_description, "");
    paymentDetails.Add(MOLPay.mp_bill_name, "");
    paymentDetails.Add(MOLPay.mp_bill_email, "");
    paymentDetails.Add(MOLPay.mp_bill_mobile, "");
    paymentDetails.Add(MOLPay.mp_channel_editing, false); // Option to allow channel selection.
    paymentDetails.Add(MOLPay.mp_editing_enabled, false); // Option to allow billing information editing.

    // Optional for Escrow
    paymentDetails.Add(MOLPay.mp_is_escrow, ""); // Optional for Escrow, put "1" to enable escrow

    // Optional for credit card BIN restrictions
    String[] binlock = new String[] { "", "" };
    paymentDetails.Add(MOLPay.mp_bin_lock, binlock); // Optional for credit card BIN restrictions
    paymentDetails.Add(MOLPay.mp_bin_lock_err_msg, ""); // Optional for credit card BIN restrictions

    // For transaction request use only, do not use this on payment process
    paymentDetails.Add(MOLPay.mp_transaction_id, ""); // Optional, provide a valid cash channel transaction id here will display a payment instruction screen.
    paymentDetails.Add(MOLPay.mp_request_type, ""); // Optional, set 'Status' when performing a transactionRequest

## Start the payment module UI

    molpay.StartMolpay(paymentDetails, MolpayCallback);

## Close the payment module UI

    molpay.CloseMolpay();

    * Notes: The close event will also return a final result.

## Transaction status request service (No UI & auto close) (optional)

    molpay.TransactionRequest(paymentDetails, MolpayCallback);

## Support

Submit issue to this repository or email to our support@molpay.com

Merchant Technical Support / Customer Care : support@molpay.com<br>
Sales/Reseller Enquiry : sales@molpay.com<br>
Marketing Campaign : marketing@molpay.com<br>
Channel/Partner Enquiry : channel@molpay.com<br>
Media Contact : media@molpay.com<br>
R&D and Tech-related Suggestion : technical@molpay.com<br>
Abuse Reporting : abuse@molpay.com