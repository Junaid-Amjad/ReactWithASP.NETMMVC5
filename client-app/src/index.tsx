import ReactDOM from 'react-dom';
import 'semantic-ui-css/semantic.min.css';
import 'react-calendar/dist/Calendar.css';
import 'react-toastify/dist/ReactToastify.min.css';
import 'react-datepicker/dist/react-datepicker.css';
import App from './App/Layout/App';
import './App/Layout/styles.css';
import { store, StoreContext } from './App/stores/store';
import reportWebVitals from './reportWebVitals';
import {createBrowserHistory} from 'history';
import {Router} from 'react-router-dom';
import ScrollToTop from './App/Layout/ScrollToTop';
import React from 'react';

export const history = createBrowserHistory();

ReactDOM.render(
    <StoreContext.Provider value={store}>
      <Router history={history}>
        <ScrollToTop />
        <App />
      </Router>      
    </StoreContext.Provider>
    ,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
