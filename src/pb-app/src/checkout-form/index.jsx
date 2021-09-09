import React, { useState } from 'react';
import {ElementsConsumer, CardElement} from '@stripe/react-stripe-js';

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
    responseModel.error = "Internal Server Error :(";
  }
  return responseModel;
}

const CheckoutForm = ({ onResult, stripe, elements }) => {

  // constructor(props) {
  //   super(props);
  //   this.state = {
  //     dollarAmount: ""
  //   };
    // this.onResult = this.props.onResult.bind(this);
  // }
  const [dollarAmount, setDollarAmount] = useState("");
  const [formMessage, setFormMessage] = useState("");
  const [clientSecret, setClientSecret] = useState("");
  const [paymentIntentID, setPaymentIntentID] = useState("");

  const submitClick = async (amount) => {
    setFormMessage("");
    if (dollarAmount == "" ) {
      setFormMessage("Please enter some money!");
    }
    let responseModel = await paymentSetup(amount)
    if (responseModel.error) {
      setFormMessage(responseModel.error);
      return;
    }
    setClientSecret(responseModel.client_secret);
    setPaymentIntentID(responseModel.payment_intent_id);
    // setFormMessage(responseModel.client_secret + " + " + responseModel.payment_intent_id)

    // TODO add handleSubmit function call
  }

  const handleSubmit = async (dollarAmount) => {

    if (!stripe || !elements) {
      return;
    }

    // Change {CLIENT_SECRET}
    const result = await stripe.confirmCardPayment('{CLIENT_SECRET}', {
      payment_method: {
        card: elements.getElement(CardElement),
        billing_details: {
          name: 'Lukey Parky',
        },
      }
    });

    if (result.error) {
      // Show error to your customer (e.g., insufficient funds)
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