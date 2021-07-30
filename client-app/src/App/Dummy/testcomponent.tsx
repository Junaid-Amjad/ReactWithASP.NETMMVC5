
import React from 'react'
import ReactPlayer from 'react-player'
import { Table } from 'semantic-ui-react'
import { ISearchFile } from '../Models/searchFile'

interface IDummy {
    dumArray: ISearchFile[]
}


export default function TableDummy({ dumArray }: IDummy) {
    let indextoiterate = 0;
    let numberofcolumns = 1;
    let runningindex = 0;

    return (
        <Table>
            <Table.Body>
                {dumArray.map((value, index) => {
                    numberofcolumns = 1;
                    if (index >= runningindex) {
                        return (
                            <Table.Row key={indextoiterate}>
                                {
                                    dumArray.map((valueinside, indexinside) => {
                                        if (numberofcolumns > 3) {

                                        }
                                        else {
                                            indextoiterate++;
                                            if (indexinside >= runningindex) {
                                                numberofcolumns++;
                                                runningindex++;
                                                return (
                                                    <Table.Cell key={valueinside.fileName}>
                                                        <ReactPlayer key={valueinside.fileName} controls url={valueinside.fullFolderandFileName} width='100%' />
                                                    </Table.Cell>
                                                )

                                            }

                                        }
                                    })
                                }
                            </Table.Row>

                        )
                    }

                })
                }
            </Table.Body>
        </Table>


    )

}

