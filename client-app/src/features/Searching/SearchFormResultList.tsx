import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import ReactPlayer from "react-player";
import { Grid, Pagination, Table } from "semantic-ui-react";
import { ISearchFile } from "../../App/Models/searchFile";
import { useStore } from "../../App/stores/store";

interface ISearchResulFile {
  SearchFile: ISearchFile[];
}

export default function SearchFormResultList({ SearchFile }: ISearchResulFile) {
  const { searchFileStore } = useStore();
  const { isRefreshArray, setIsRefreshvalue } = searchFileStore;
  const [searchSliceData, setSearchSliceData] =
    useState<ISearchFile[]>() || undefined;
  const [searchSliceActivePage, setSearchSliceActivePage] = useState(1);
  const noOfRecords = 6;
  const totalPages = Math.ceil(SearchFile.length / noOfRecords);
  let indextoiterate = 0;
  let numberofcolumns = 1;
  let runningindex = 0;
  const setPaginationFile = async (activePage: any) => {
    let v = SearchFile.slice(
      (activePage - 1) * noOfRecords,
      (activePage - 1) * noOfRecords + noOfRecords
    );
    setSearchSliceData(v);
    setSearchSliceActivePage(activePage);
  };

  const onChange = (e: any, pageInfo: any) => {
    setPaginationFile(pageInfo.activePage);
  };
  useEffect(() => {
    if (isRefreshArray) {
      let v = SearchFile.slice(0 * noOfRecords, 0 * noOfRecords + noOfRecords);
      setSearchSliceData(v);
      setSearchSliceActivePage(1);
      setIsRefreshvalue();
    }
  }, [isRefreshArray, SearchFile, setSearchSliceData, setIsRefreshvalue]);

  return (
    <>
      <Grid>
        <Grid.Column textAlign="center">
          <Pagination
            totalPages={totalPages}
            activePage={searchSliceActivePage}
            onPageChange={onChange}
          />
        </Grid.Column>
      </Grid>
      <Table>
        <Table.Body>
          {(searchSliceData! || []).map((value, index) => {
            numberofcolumns = 1;
            if (index >= runningindex) {
              return (
                <Table.Row key={indextoiterate}>
                  {(searchSliceData! || []).map((valueinside, indexinside) => {
                    if (numberofcolumns > 3) {
                      return null;
                    } else {
                      indextoiterate++;
                      if (indexinside >= runningindex) {
                        numberofcolumns++;
                        runningindex++;
                        return (
                          <Table.Cell key={valueinside.fileName}>
                            <ReactPlayer
                              key={valueinside.fileName}
                              controls
                              url={valueinside.fullFolderandFileName}
                              width="100%"
                            />
                          </Table.Cell>
                        );
                      } else {
                        return null;
                      }
                    }
                  })}
                </Table.Row>
              );
            } else {
              return null;
            }
          })}
        </Table.Body>
      </Table>
    </>
  );
}
