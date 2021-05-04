import { ErrorMessage, Formik } from "formik";
import { observer } from "mobx-react-lite";
import { Button, Form, Header } from "semantic-ui-react";
import MyTextInput from "../../App/common/form/MyTextInput";
import { useStore } from "../../App/stores/store";
import * as Yup from 'yup'
import ValidationError from "../errors/ValidationError";

export default observer(function RegisterForm(){
    const {userStore} = useStore();
    return(
        <Formik 
        initialValues={{displayName:'',userName:'',email:'',password:'',error:null}}
        onSubmit={(values,{setErrors}) => userStore.register(values).catch(error => setErrors({error})) }
        validationSchema={Yup.object({
            displayName: Yup.string().required(),
            userName: Yup.string().required(),
            email: Yup.string().required().email(),
            password: Yup.string().required(),
        })}
        >
            {({handleSubmit,isSubmitting,errors,isValid,dirty}) => (
                <Form className='ui form error' onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' content='Sign Up to the Application' color='teal' textAlign='center' />
                    <MyTextInput name='displayName' placeholder='Display Name' />
                    <MyTextInput name='userName' placeholder='User Name' />
                    <MyTextInput name='email' placeholder='Email' />
                    <MyTextInput name='password' placeholder='Password' type='password' />
                    <ErrorMessage 
                        name='error' 
                        render={() => 
                            <ValidationError errors={errors.error} />
                            }
                    />
                    <Button disabled={!isValid || !dirty || isSubmitting} loading={isSubmitting} positive content='Register' type='submit' fluid />
                </Form>
            )}

        </Formik>
    )

})