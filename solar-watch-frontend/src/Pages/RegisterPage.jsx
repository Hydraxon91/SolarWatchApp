import React, { useEffect, useRef } from 'react';
import RegisterElement from "../Components/RegisterElement";
import { Link, useNavigate } from 'react-router-dom';
import "../styles/lotty.css";

export default function RegisterPage(){
    return(
            <div className="register-page-element-container">
                <RegisterElement></RegisterElement>
            </div>
    
        )
    }