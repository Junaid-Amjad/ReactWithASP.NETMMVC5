import { Formik } from "formik"
import { observer } from "mobx-react-lite"
import * as Yup from 'yup';
import { Button, Form, Grid, Header, Label, Radio, Segment, } from "semantic-ui-react"
import MyDateInput from "../../App/common/form/MyDateInput";
import React, { useState } from "react";

interface SearchForm {
    DateFrom: Date | null;
    TimeFrom: Date | null;
    DateTo: Date | null;
    TimeTo: Date | null;
    OrderByID: Number;

}

export default observer(function SearchForm() {


    const validationSchema = Yup.object({
        datefrom: Yup.string().required('Date From is Required').nullable(),
        timefrom: Yup.string().required('Time From is required').nullable(),
        dateto: Yup.string().required('Date To is Required').nullable(),
        timeto: Yup.string().required('Time To is Required').nullable(),
    })
    const [searchform, setSearchForm] = useState<SearchForm>({
        DateFrom: null,
        TimeFrom: null,
        DateTo: null,
        TimeTo: null,
        OrderByID: 0

    });

    function handleFormSubmit(activity: SearchForm) {
        console.log('After Searching');



        //        if (activity.activityID.length === 0) {
        //            let newActivity = {
        //                ...activity,
        //                activityID: uuid()
        //            }
        //            createActivity(newActivity).then(() => history.push(`/activities/${newActivity.activityID}`))

        //        } else {
        //           updateActivity(activity).then(() => history.push(`/activities/${activity.activityID}`))
        //       }
    }

    return (
        <Segment clearing>
            <Header content='Searching and Display Options' />
            <Formik
                validationSchema={validationSchema}
                enableReinitialize initialValues={searchform}
                onSubmit={values => handleFormSubmit(values)}>
                {({ handleSubmit, isValid, isSubmitting, dirty }) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <Grid columns={2}>
                            <Grid.Column>
                                <Grid.Row>
                                    <Segment raised>
                                        <Label content='Date From' color='blue' ribbon />
                                        <MyDateInput
                                            dateFormat='MMMM dd, yyyy'
                                            name='datefrom'
                                            placeholderText='Date From'
                                        />
                                        <Label content='Time From' color='blue' ribbon />
                                        <MyDateInput
                                            dateFormat='h:mm aa'
                                            name='timefrom'
                                            placeholderText='Time From'
                                            showTimeSelectOnly
                                            showTimeInput
                                        />
                                    </Segment>

                                </Grid.Row>
                            </Grid.Column>
                            <Grid.Column>
                                <Grid.Row>
                                    <Segment raised>
                                        <Label content='Date To' color='blue' ribbon />
                                        <MyDateInput
                                            dateFormat='MMMM dd, yyyy'
                                            name='dateto'
                                            placeholderText='Date To'
                                        />
                                        <Label content='Time To' color='blue' ribbon />
                                        <MyDateInput
                                            dateFormat='h:mm aa'
                                            name='timeto'
                                            placeholderText='Time To'
                                            showTimeSelectOnly
                                            showTimeInput
                                        />
                                    </Segment>
                                </Grid.Row>
                            </Grid.Column>
                        </Grid>
                        <Grid columns={1}>
                            <Grid.Column>
                                <Grid.Row>
                                    <Segment raised>
                                        <Label content='Order By' color='blue' ribbon />

                                        <Form.Group widths='equal'>
                                            <Form.Field>
                                                <Radio slider
                                                    label='Date Asc'
                                                    name="orderby"
                                                    checked
                                                />

                                            </Form.Field>
                                            <Form.Field>
                                                <Radio slider
                                                    label='Date Desc'
                                                    name="orderby"
                                                />

                                            </Form.Field>
                                            <Form.Field>
                                                <Radio slider
                                                    label='Length Asc'
                                                    name="orderby"
                                                />

                                            </Form.Field>
                                            <Form.Field>
                                                <Radio slider
                                                    label='Length Desc'
                                                    name="orderby"
                                                />

                                            </Form.Field>
                                        </Form.Group>

                                    </Segment>
                                </Grid.Row>
                            </Grid.Column>
                            <Grid.Column>
                                <Button content='Search'
                                    disabled={isSubmitting || !dirty || !isValid}
                                    positive type='submit' />

                            </Grid.Column>
                        </Grid>

                    </Form>
                )}
            </Formik>
        </Segment>
    )

})
