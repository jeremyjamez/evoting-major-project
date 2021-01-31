import {Grid, Input} from "@geist-ui/react";

const FilterComponent = ({ filterText, onFilter, onClear }) => (
    <>
        <Grid.Container justify="flex-start">
            <Grid xs={24}>
                <Input id="search" placeholder="Search" aria-label="Search Input" value={filterText} onChange={onFilter} onClearClick={onClear} size="large" width="40%" clearable />
            </Grid>
        </Grid.Container>
    </>
);

export default FilterComponent;