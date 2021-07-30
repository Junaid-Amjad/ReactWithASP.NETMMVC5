
import React from 'react'
import ReactPlayer from 'react-player'
import { Table } from 'semantic-ui-react'
import { ISearchFile } from '../../App/Models/searchFile';

interface ISearchResulFile {
    SearchFile: ISearchFile[]
}


export default function SearchFormResultList({ SearchFile }: ISearchResulFile) {
    let indextoiterate = 0;
    let numberofcolumns = 1;
    let runningindex = 0;
    
    return (
        <Table>
            <Table.Body>
                {SearchFile.map((value, index) => {
                    numberofcolumns = 1;
                    if (index >= runningindex) {
                        return (
                            <Table.Row key={indextoiterate}>
                                {
                                    SearchFile.map((valueinside, indexinside) => {
                                        if (numberofcolumns > 3) {
                                            return (null)
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
                                            else {
                                                return (null)
                                            }

                                        }
                                    })
                                }
                            </Table.Row>

                        )
                    }
                    else {
                        return (null)
                    }

                })
                }
            </Table.Body>
        </Table>
    )

}

