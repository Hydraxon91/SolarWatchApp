import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';
import {BrowserRouter, createBrowserRouter, RouterProvider, Route} from 'react-router-dom';
import RegisterPage from './Pages/RegisterPage';
import FrontPage from './Pages/FrontPage';

const router = createBrowserRouter([
  {
    path: "/",
    errorElement: <FrontPage />,
    
    children: [
      {
        path: "/",
        element: <FrontPage />,
      },
      {
        path: "/registration",
        element: <RegisterPage />,
      }
    ]
  }
]);

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <RouterProvider router={router}/>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
