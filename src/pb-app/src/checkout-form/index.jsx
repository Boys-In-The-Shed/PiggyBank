import React from 'react';
import {ElementsConsumer, CardElement} from '@stripe/react-stripe-js';

import CardSection from '../card-section/index.jsx';
import ButtonBase from '../button-base/index.jsx';
import './checkout-form.css';

class CheckoutForm extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      dollarAmount: ""
    };
  }

  handleSubmit = async (dollarAmount) => {
    const {stripe, elements} = this.props;

    if (!stripe || !elements) {
      return;
    }

    const result = await stripe.confirmCardPayment('{CLIENT_SECRET}', {
      payment_method: {
        card: elements.getElement(CardElement),
        billing_details: {
          name: 'Jenny Rosen',
        },
      }
    });

    if (result.error) {
      // Show error to your customer (e.g., insufficient funds)
      console.log(result.error.message);
    } else {
      // The payment has been processed!
      if (result.paymentIntent.status === 'succeeded') {
        // Show a success message to your customer
        // There's a risk of the customer closing the window before callback
        // execution. Set up a webhook or plugin to listen for the
        // payment_intent.succeeded event that handles any business critical
        // post-payment actions.
      }
    }
  };

  render() {
    return (
      <form>
        <div className="checkoutForm">
          <div>$</div>
          <input type="number" onChange={e => this.setState({ dollarAmount: e.target.value })}></input>
        </div>
        <CardSection />
        <ButtonBase userClick={() => this.handleSubmit(this.state.dollarAmount)}>PAY US!</ButtonBase>
      </form>
    );
  }
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