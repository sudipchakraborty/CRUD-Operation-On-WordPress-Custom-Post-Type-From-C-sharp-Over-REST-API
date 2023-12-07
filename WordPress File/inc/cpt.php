<?php
add_action('rest_api_init','cpt_Handle');

global $wpdb;
$charset_collate = $wpdb->get_charset_collate();
require_once(ABSPATH . 'wp-admin/includes/upgrade.php');
/* ________________________________________________________________________________________________________________*/
function cpt_Handle()
{
    register_rest_route('bp','v3',array(
        'methods'=>'GET',
        'callback'=>'cpt_cmd_Execute'
    ));

    register_rest_route('bp','v3',array(
        'methods'=>'POST',
        'callback'=>'cpt_cmd_Execute'
    ));
}
/* ________________________________________________________________________________________________________________*/
function cpt_cmd_Execute($request) 
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
        global $wpdb;         
        $posts = $wpdb->get_results($cmd_str);
        return $posts;
    }  
    ////////////////////////////////
    else if($cmd=="delete_row")
    {
        $result = wp_delete_post($request['id'], true);  

        if ($result !== false) {
           return "Successfully Deleted";
        } else {
            return "Unable to Delete Post";
        }
    }
    ////////////////////////////////
    else
    {
        return "Other DB Operation command";
    }
}