import React, { useEffect, useRef } from 'react';
import LoginElement from "../Components/LoginElement";
import { Link, useNavigate } from 'react-router-dom';
import "../styles/login.css";

export default function LoginPage(props){

    return(
            <div className="login-page-element-container">
                <LoginElement cookies={props.cookies} setUser={props.setUser} user={props.user}></LoginElement>
            </div>
        )
    }