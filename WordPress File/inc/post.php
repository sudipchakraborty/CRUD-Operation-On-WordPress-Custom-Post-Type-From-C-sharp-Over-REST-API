<?php
add_action('rest_api_init','post_Handle');

global $wpdb;
$charset_collate = $wpdb->get_charset_collate();
require_once(ABSPATH . 'wp-admin/includes/upgrade.php');
/* ________________________________________________________________________________________________________________*/
function post_Handle()
{
    register_rest_route('bp','v2',array(
        'methods'=>'GET',
        'callback'=>'post_cmd_Execute'
    ));

    register_rest_route('bp','v2',array(
        'methods'=>'POST',
        'callback'=>'post_cmd_Execute'
    ));
}
/* ________________________________________________________________________________________________________________*/
function post_cmd_Execute($request) 
{
    $cmd= $request["cmd"];
    $cmd_str=$request["cmd_string"]; 
    ////////////////////////////////
    if($cmd=="connect")
    {
        Return 'ok';
    }
    ////////////////////////////////
    else if($cmd=="create")
    {
    }
    ////////////////////////////////
    else if($cmd=="insert")
    {
        $new_post = array(
            'post_title'    => 'bp', 
            'post_content'  => $cmd_str, 
            'post_status'   => 'publish', 
            'post_author'   => 1, 
            'post_type'     => 'post',
            'post_category' => array(4,4)
        );
        $post_id = wp_insert_post($new_post);       
        if ($post_id){
            return "Insert Successfully";
        } else {
            return "Failed to insert post.";
        }
    }
    ////////////////////////////////5
    else if($cmd=="read")
    {
        global $wpdb;         
        $posts = $wpdb->get_results($cmd_str);
        return $posts;
    }
    ////////////////////////////////
    else if($cmd=="update")
    {
        // Prepare updated post data
        $updated_post = array(
        'ID'           => $request['id'],
        'post_content' => $cmd_str,
        );
        // Update the post
        $post_updated = wp_update_post($updated_post);

        if ($post_updated !== 0) 
        {
           return "Successfully Updated";
        } else 
        {
            return "Unable to Updat Post";
        }  
    }  
    ////////////////////////////////
    else if($cmd=="delete_row")
    {
        $post_id = 12; // Replace with your post ID
        $result = wp_delete_post($request['id'], true); // Set second parameter to true for force delete, false for move to trash

        if ($result !== false) {
           // echo "Post deleted successfully!";
           return "Successfully Deleted";
        } else {
            return "Unable to Delete Post";
           // echo "Failed to delete post.";
        }


        // global $wpdb;

        // if (!empty($request['id']))
        // {
        //     $wpdb->delete
        //     ( 
        //         $request["tbl_name"], 
        //         array('id' => $request['id']) 
        //     );              
        // }           
        // return "Successfully Deleted";
    }
    ////////////////////////////////
    else if($cmd=="delete_table")
    {
        global $wpdb;

        $posts = $wpdb->get_results("drop table wp_bp2");

        // $wpdb->delete
        // ( 
        //     $request["tbl_name"]
        // );                            
        return $posts;//"Table Deleted";
    }
    ////////////////////////////////
    else
    {
        return "Other DB Operation command";
    }
}