import React, { useEffect, useRef } from 'react';
import RegisterElement from "../Components/RegisterElement";
import "../styles/lotty.css";

export default function RegisterPage(){
    return(
            <div className="register-page-element-container">
                <RegisterElement></RegisterElement>
            </div>
    
        )
    }