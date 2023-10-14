import React, { useEffect, useRef, useState } from 'react';
import "../styles/clockanimation.css";

const AnimatedClock = ({time, solarType}) =>{
    // console.log(time)
    const splitTime = time.split(':');
    const hour = splitTime[0];
    const minute = splitTime[1];
    const second = splitTime[2];

    const hRotate = 30 * hour + 0.5 * minute;
    const mRotate = 6 * minute;
    const sRotate = 6 * second;

    return (
        <div className='clock-animation-container'>
            <section className={`clock ${solarType}`}>
            
                <div className="hands firsthand" style={{ transform: `rotate(${hRotate}deg)` }}><i></i></div>
                <div className="hands secondhand" style={{ transform: `rotate(${mRotate}deg)` }}><i></i></div>
                <div className="hands thirdhand" style={{ transform: `rotate(${sRotate}deg)` }}><i></i></div>
                
                <span style={{ "--i": 1 }}><b>1</b></span>
                <span style={{ "--i": 2 }}><b>2</b></span>
                <span style={{ "--i": 3 }}><b>3</b></span>
                <span style={{ "--i": 4 }}><b>4</b></span>
                <span style={{ "--i": 5 }}><b>5</b></span>
                <span style={{ "--i": 6 }}><b>6</b></span>
                <span style={{ "--i": 7 }}><b>7</b></span>
                <span style={{ "--i": 8 }}><b>8</b></span>
                <span style={{ "--i": 9 }}><b>9</b></span>
                <span style={{ "--i": 10 }}><b>10</b></span>
                <span style={{ "--i": 11 }}><b>11</b></span>
                <span style={{ "--i": 12 }}><b>12</b></span>
            </section>
        </div>
    );
}

export default AnimatedClock;