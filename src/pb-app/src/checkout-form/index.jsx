import React, { useState } from 'react';
import {useStripe, useElements, CardElement} from '@stripe/react-stripe-js';

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

async function paymentConfirm(paymentIntentID) {
  const response = await fetch('https://api.piggybank.lukejoshuapark.io/payment/confirm', {
    method: 'POST',
    mode: 'cors', 
    headers: {
      'content-type': 'application/json'
    },
    redirect: 'follow',
    referrerPolicy: 'no-referrer',
    body: JSON.stringify({payment_intent_id : paymentIntentID})
  });
  const responseModel =  await response.json();
  if (response.status !== 200 && !responseModel.error) {
    responseModel.error = "Something went wrong.";
  }
  return responseModel;
}

const CheckoutForm = ({ updateBalance }) => {
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
    
    handleSubmit(responseModel.payment_intent_id, responseModel.client_secret);
    return;
  }

  const handleSubmit = async (paymentIntentID, clientSecret) => {

    if (!stripe || !elements) {
      return;
    }

    const result = await stripe.confirmCardPayment(clientSecret, {
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
      return;
    } else {
      if (result.paymentIntent.status === 'succeeded') {
        setFormMessage("Successful!");
        let responseModel = await paymentConfirm(paymentIntentID);
        if (responseModel.error) {
          setFormMessage(responseModel.error);
          console.log(responseModel.error);
          return;
        }
        updateBalance(responseModel.current_balance);
        
        // Show a success message to your customer
        // There's a risk of the customer closing the window before callback
        // execution. Set up a webhook or plugin to listen for the
        // payment_intent.succeeded event that handles any business critical
        // post-payment actions.
      }
      return;
    }
  };

  return (
    <form>
      <div className="checkoutForm">
        <div>$</div>
        <input type="number" onChange={e => setDollarAmount(e.target.value)}></input>
      </div>
      <CardSection />
      <div className='message-button-container'>
        <div className='form-message'>{formMessage}</div>
        <ButtonBase userClick={() => submitClick(dollarAmount)}>PAY US!</ButtonBase>
      </div>
    </form>
  );
}

export default CheckoutForm;