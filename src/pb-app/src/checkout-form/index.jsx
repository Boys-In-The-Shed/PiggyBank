import React, { useState } from 'react';
import {useStripe, useElements, ElementsConsumer, CardElement} from '@stripe/react-stripe-js';

import CardSection from '../card-section/index.jsx';
import ButtonBase from '../button-base/index.jsx';
import './checkout-form.css';

async function paymentSetup(amount) {
  const response = await fetch('https://api.piggybank.lukejoshuapark.io/payment/setup', {
    method: 'POST',
    mode: 'cors', 
    headers: {
      'content-type': 'application/json'
    },
    redirect: 'follow',
    referrerPolicy: 'no-referrer',
    body: JSON.stringify({amount: parseFloat(amount)})
  });
  const responseModel =  await response.json();
  if (response.status !== 200 && !responseModel.error) {
    responseModel.error = "Something went wrong.";
  }
  return responseModel;
}

const CheckoutForm = () => {
  const stripe = useStripe();
  const elements = useElements();
  const [dollarAmount, setDollarAmount] = useState("");
  const [formMessage, setFormMessage] = useState("");

  const submitClick = async (amount) => {
    setFormMessage("");
    if (dollarAmount === "" ) {
      setFormMessage("Please enter some money!");
      return;
    }
    let responseModel = await paymentSetup(amount)
    if (responseModel.error) {
      setFormMessage(responseModel.error);
      console.log(responseModel.error);
      return;
    }
    console.log("clientSecret = " + responseModel.client_secret + " AND paymentIntentID = " + responseModel.payment_intent_id);

    // TODO add handleSubmit function call
  }

  const handleSubmit = async () => {

    if (!stripe || !elements) {
      return;
    }

    // TODO Change {CLIENT_SECRET}
    const result = await stripe.confirmCardPayment('{CLIENT_SECRET}', {
      payment_method: {
        card: elements.getElement(CardElement),
        billing_details: {
          name: 'Lukey Parky',
        },
      }
    });

    if (result.error) {
      setFormMessage(result.error.message);
      console.log(result.error.message);
    } else {
      // The payment has been processed!
      if (result.paymentIntent.status === 'succeeded') {
        setFormMessage("Successful!");
        // TODO POST successful payment to server
        
        // Show a success message to your customer
        // There's a risk of the customer closing the window before callback
        // execution. Set up a webhook or plugin to listen for the
        // payment_intent.succeeded event that handles any business critical
        // post-payment actions.
      }
    }
  };

  return (
    <form>
      <div className="checkoutForm">
        <div>$</div>
        <input type="number" onChange={e => setDollarAmount(e.target.value)}></input>
      </div>
      <CardSection />
      <div>
        <div className='form-message'>{formMessage}</div>
        <ButtonBase userClick={() => submitClick(dollarAmount)}>PAY US!</ButtonBase>
      </div>
    </form>
  );
}

export default function InjectedCheckoutForm() {
  return (
    <ElementsConsumer>
      {({stripe, elements}) => (
        <CheckoutForm  stripe={stripe} elements={elements} />
      )}
    </ElementsConsumer>
  );
}