import React, { useState, useEffect } from 'react';
import SolarInfoElement from '../Components/SolarInfoElement';
import "../styles/lotty.css";
import "../styles/solarwatchpage.css";

export default function SolarWatchElement({cookies, setNewStopFrame}){
    const [fetchCity, setFetchCity] = useState(null);
    const [fetchDate, setFetchDate] = useState(null);

    const [fetchData, setFetchData] = useState(null);
    const [sunriseData, setSunriseData] = useState(null);
    const [sunsetData, setSunsetData] = useState(null);
    const [noonData, setNoonData] = useState(null);
    const [date, setDate] = useState(null);

    const [firstTimeLoading, setFirstTimeLoading] = useState(true);

    

    const handleSubmit = (e) => { 
        e.preventDefault(); 
        var jwtAuthorication = cookies.get("jwt_authorization");
        setSunriseData(null);
        setFirstTimeLoading(false);
        fetch(`http://localhost:8082/GetSunriseSunset?city=${fetchCity}&date=${fetchDate}`, {
        method: 'GET',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'text/plain',
            'Authorization': 'Bearer ' + jwtAuthorication
        }
        })
            .then(res => res.json())
            .then(r => { setFetchData(r)})
            .catch(err=>console.error(err))
    };
    const splitDataToString = (fullData) =>{
        return fullData.split(' ')[0];
    }

    const splitDate = (fullDate) =>{
        const datePart = fullDate.split('T')[0];
        return datePart.replace(/-/g, '/');
    }

    useEffect(()=>{
        if (fetchData?.id) {
            setDate(splitDate(fetchData.date));
            setSunriseData(splitDataToString(fetchData.sunrise));
            setSunsetData(splitDataToString(fetchData.sunset));
            setNoonData(splitDataToString(fetchData.solarNoon));
        }
    },[fetchData])

    return(
           
        <div className='solar-page-element-container'>
            <form className='solarwatch-request-form'  onSubmit={handleSubmit}>
                    <div className="data-inputboxholder">
                        <div className="city-input-box">
                            <input type="text" required onChange={(e) => setFetchCity(e.target.value)}></input>
                            <label for="">City</label>
                        </div>
                        <div className="date-input-box">
                            <input type="date" required onChange={(e) => setFetchDate(e.target.value)} value={fetchDate}></input>
                        </div>
                    </div>
                    <button type="submit" className="submitfetch-button">Send Get Request</button>
            </form>
            
            {
                !firstTimeLoading &&(
                    sunriseData ? (
                        <div className='solar-outer-container'>
                        <div className='req-date'>Here is the data for: {date}</div>
                        <div className='solar-info-container'>
                            <SolarInfoElement setNewStopFrame={setNewStopFrame} solarData={sunriseData} solarType={"sunrise"}></SolarInfoElement>
                            <SolarInfoElement setNewStopFrame={setNewStopFrame} solarData={noonData} solarType={"noon"}></SolarInfoElement>
                            <SolarInfoElement setNewStopFrame={setNewStopFrame} solarData={sunsetData} solarType={"sunset"}></SolarInfoElement>
                        </div>
                        </div>
                    ) : (
                        <div className={`loading-div`}></div>
                    )
                )
            }
        </div>
    
        )
    }