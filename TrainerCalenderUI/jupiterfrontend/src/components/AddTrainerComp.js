import React from "react";
import { useFormik } from "formik";
import * as Yup from 'yup'
import "../styles/style.css"
import { useNavigate } from "react-router-dom";

function TrainerComp(){
    
    const navigate = useNavigate();

    const formik = useFormik({

        initialValues :{
            id : "",
            name : "",
            email : "",
            contactNo : ""
        },

        validationSchema : Yup.object({
              id : Yup.number().positive("Invalid ID").required("ID is a required field"),
              name : Yup.string().required("Name is a required field"),
              email : Yup.string().email("Invalid email").required("Email is a required field"),
              contactNo : Yup.number().min(10,'Invalid Contact Number').positive("Invalid contact number").required("ContactNumber is a required field"),
        }),

        onSubmit:(values)=>{
            console.log(values)
        }
    })

    const handleBackClick=()=>{
             navigate("/home")
    }

    return(
        <div className="form">
            <form onSubmit={formik.handleSubmit}>
            <h2>Add a trainer</h2>

            <label htmlFor="id">ID</label>
            <input
                id="id"
                type="text"
                {...formik.getFieldProps('id')}
            />
            {formik.touched.id && formik.errors.id ? (
                <div style={{color:'red'}}>{formik.errors.id}</div>
            ) : null}
            <br></br>
            <label htmlFor="name">Name</label>
            <input
                id="name"
                type="text"
                {...formik.getFieldProps('name')}
            />
            {formik.touched.name && formik.errors.name ? (
                <div style={{color:'red'}}>{formik.errors.name}</div>
            ) : null}
            <br></br>
            <label htmlFor="email">Email</label>
            <input
                id="email"
                type="text"
                {...formik.getFieldProps('email')}
            />
            {formik.touched.email && formik.errors.email ? (
                <div style={{color:'red'}}>{formik.errors.email}</div>
            ) : null}
            <br></br>

            <label htmlFor="contactNo">Contact number</label>
            <input
                id="contactNo"
                type="text"
                {...formik.getFieldProps('contactNo')}
            />
            {formik.touched.contactNo && formik.errors.contactNo ? (
                <div style={{color:'red'}}>{formik.errors.contactNo}</div>
            ) : null}

            <div className="btn1">
            <button type="submit" className="btn1">Register</button>&nbsp;&nbsp;&nbsp;&nbsp;
            <button type="reset" className="btn2" onClick={handleBackClick}>Back</button>
            </div>

        </form>

        </div>
    )
}

export default TrainerComp;