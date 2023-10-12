import { Link } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import "../styles/lotty.css";

export default function SolarWatchPage(props){
    //GetSunriseSunset?city=Budapest&date=1996-12-13
    const [fetchData, setFetchData] = useState(null);

    const handleSubmit = () => {  
        var jwtAuthorication = props.cookies.get("jwt_authorization");
        console.log(jwtAuthorication);
        fetch('http://localhost:8082/GetSunriseSunset?city=Budapest&date=1996-12-13', {
        method: 'GET',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'text/plain',
            'Authorization': 'Bearer ' + jwtAuthorication
        }
        })
            .then(res => res.json())
            .then(r => { setFetchData(JSON.stringify(r))})
            .catch(err=>console.error(err))
    };

    return(
           
        <div className='page-element-container'>
            <button onClick={handleSubmit}>Send Get Request</button>
            {fetchData}
        </div>
    
        )
    }