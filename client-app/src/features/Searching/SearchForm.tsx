import { Formik } from "formik"
import { observer } from "mobx-react-lite"
import * as Yup from 'yup';
import { Button, Divider, Form, Grid, Header, Label, Radio, Segment, } from "semantic-ui-react"
import MyDateInput from "../../App/common/form/MyDateInput";
import React, { useState } from "react";
import { useStore } from "../../App/stores/store";
import SearchFormResultList from "./SearchFormResultList";

interface SearchForminterface {
    DateFrom: Date | null;
    TimeFrom: Date | null;
    DateTo: Date | null;
    TimeTo: Date | null;
    OrderByID: Number;
    StartTickDate: Number;
    EndTickDate: Number;

}

export default observer(function SearchForm() {
    const { searchFileStore } = useStore();
    const { getSearchFilterFiles, savedFileSearchedRegistry, loadingSearchingFile, isLoadingContent } = searchFileStore;
    let orderbyidlocal = 1;

    const validationSchema = Yup.object({
        DateFrom: Yup.string().required('Date From is Required').nullable(),
        TimeFrom: Yup.string().required('Time From is required').nullable(),
        DateTo: Yup.string().required('Date To is Required').nullable(),
        TimeTo: Yup.string().required('Time To is Required').nullable(),
    })
    const [searchform, setsearchform] = useState<SearchForminterface>({
        DateFrom: null,
        TimeFrom: null,
        DateTo: null,
        TimeTo: null,
        OrderByID: 0,
        StartTickDate: 0,
        EndTickDate: 0

    });

    const handleInputChange = (score: any) => {
        orderbyidlocal = score;
    };


    async function handleFormSubmit(searchformparameter: SearchForminterface) {

        searchformparameter.OrderByID = orderbyidlocal;

        var timefrom = (searchformparameter.TimeFrom!.getHours() < 10 ? '0' : '') + searchformparameter.TimeFrom!.getHours() + ':' +
            (searchformparameter.TimeFrom!.getMinutes() < 10 ? '0' : '') + searchformparameter.TimeFrom!.getMinutes() + ':' +
            (searchformparameter.TimeFrom!.getSeconds() < 10 ? '0' : '') + searchformparameter.TimeFrom!.getSeconds();


        var timeto = (searchformparameter.TimeTo!.getHours() < 10 ? '0' : '') + searchformparameter.TimeTo!.getHours() + ':' +
            (searchformparameter.TimeTo!.getMinutes() < 10 ? '0' : '') + searchformparameter.TimeTo!.getMinutes() + ':' +
            (searchformparameter.TimeTo!.getSeconds() < 10 ? '0' : '') + searchformparameter.TimeTo!.getSeconds();


        var datefrom = new Date(searchformparameter.DateFrom?.toDateString() + ' ' + timefrom.toString());
        var dateto = new Date(searchformparameter.DateTo?.toDateString() + ' ' + timeto.toString());


        var starttickdate = datefrom.getTime();
        var endtickdate = dateto.getTime();

        searchformparameter.StartTickDate = starttickdate;
        searchformparameter.EndTickDate = endtickdate;

        await getSearchFilterFiles(searchformparameter);
        setsearchform(searchformparameter);
    }



    return (
        <>
            <Segment clearing>
                <Header content='Searching and Display Options' />
                <Divider />
                <Formik
                    validationSchema={validationSchema}
                    enableReinitialize initialValues={searchform}
                    onSubmit={values => handleFormSubmit(values)}>
                    {({ handleSubmit, isValid, isSubmitting, dirty }) => (
                        <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                            {/*InputFields*/}
                            <Grid columns={4} stackable>
                                <Grid.Column>
                                    <Grid.Row>
                                        <Segment raised>
                                            <Label content='Date From' color='blue' ribbon />
                                            <MyDateInput
                                                dateFormat='MMMM dd, yyyy'
                                                name='DateFrom'
                                                placeholderText='Date From'
                                            />
                                        </Segment>
                                    </Grid.Row>
                                </Grid.Column>
                                <Grid.Column>
                                    <Grid.Row>
                                        <Segment raised>
                                            <Label content='Time From' color='blue' ribbon />
                                            <MyDateInput
                                                dateFormat='h:mm aa'
                                                name='TimeFrom'
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
                                                name='DateTo'
                                                placeholderText='Date To'
                                            />
                                        </Segment>
                                    </Grid.Row>
                                </Grid.Column>
                                <Grid.Column>
                                    <Grid.Row>
                                        <Segment raised>
                                            <Label content='Time To' color='blue' ribbon />
                                            <MyDateInput
                                                dateFormat='h:mm aa'
                                                name='TimeTo'
                                                placeholderText='Time To'
                                                showTimeSelectOnly
                                                showTimeInput
                                            />
                                        </Segment>
                                    </Grid.Row>
                                </Grid.Column>
                            </Grid>

                            {/*RadioButton*/}
                            <Grid columns={1} stackable>
                                <Grid.Column>
                                    <Grid.Row>
                                        <Segment raised>
                                            <Label content='Order By' color='blue' ribbon />
                                            <Form.Group widths='equal'>
                                                <Form.Field>
                                                    <Radio slider
                                                        label='Date Asc'
                                                        name="OrderByID"
                                                        checked
                                                        onChange={e => handleInputChange(1)}
                                                    />

                                                </Form.Field>
                                                <Form.Field>
                                                    <Radio slider
                                                        label='Date Desc'
                                                        name="OrderByID"
                                                        onChange={e => handleInputChange(2)}
                                                    />

                                                </Form.Field>
                                                <Form.Field>
                                                    <Radio slider
                                                        label='Length Asc'
                                                        name="OrderByID"
                                                        onChange={e => handleInputChange(3)}
                                                    />

                                                </Form.Field>
                                                <Form.Field>
                                                    <Radio slider
                                                        label='Length Desc'
                                                        name="OrderByID"
                                                        onChange={e => handleInputChange(4)}
                                                    />

                                                </Form.Field>
                                            </Form.Group>
                                        </Segment>

                                    </Grid.Row>
                                </Grid.Column>
                                <Grid.Column>
                                    <Button content='Search'
                                        loading={loadingSearchingFile}
                                        disabled={isSubmitting || !dirty || !isValid}
                                        positive type='submit' />

                                </Grid.Column>
                            </Grid>

                        </Form>
                    )}
                </Formik>

            </Segment>
            {
                isLoadingContent ?
                    <SearchFormResultList SearchFile={savedFileSearchedRegistry} />
                    : null
            }
        </>
    )

})
